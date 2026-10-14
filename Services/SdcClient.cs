using Azure.Core;
using ERS_Intergration.Models.Dtos;
using Microsoft.Identity.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace ERS_Intergration.Services
{
    public class SdcClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SdcClient> _logger;
        private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private readonly IConfiguration _config;
        public class SdcException(string message) : Exception(message);
        public SdcClient(HttpClient httpClient, ILogger<SdcClient> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
        }

        // Send the PIN once. Do not call in a loop (PIN counter / lockout).
        public async Task SendPinAsync()
        {
            var pin = _config["Sdc:Pin"];
            if (string.IsNullOrEmpty(pin))
            {
                _logger.LogError("PIN is not configured.");
                throw new InvalidOperationException("PIN is not configured.");
            }
            try
            {
                var r = await _httpClient.PostAsync("v4/pin", new StringContent(pin, Encoding.UTF8, "application/json"));
                var body = (await r.Content.ReadAsStringAsync()).Trim().Trim('"');
                if (body != "0100") throw new SdcException($"PIN not accepted: {body}");
            }
            catch (Exception)
            {

                throw;
            }
        }
            //create the invoice to send to SDC
            public async Task<string> CreateInvoiceAsync(InvoiceRequest request, string requestId)
            {
                var json = JsonSerializer.Serialize(request, Json);
                for (var attempt = 0; ; attempt++)
                {
                    var msg = new HttpRequestMessage(HttpMethod.Post, "v4/invoices")
                    { Content = new StringContent(json, Encoding.UTF8, "application/json") };
                    msg.Headers.Add("RequestId", requestId);
                    var r = await _httpClient.SendAsync(msg);
                    var body = await r.Content.ReadAsStringAsync();
                    if (r.IsSuccessStatusCode) return body;
                    if (attempt == 0 && body.Contains("1500")) { await SendPinAsync(); continue; } // PIN required
                    throw new SdcException($"SDC rejected invoice ({(int)r.StatusCode}): {body}");

                }


            }
        // For recovery after a timeout: has this RequestId already been signed?
        public enum SdcLookup { Found, NotFound, Unavailable }
        public async Task<(SdcLookup Status, string? Body)> LookupAsync(string requestId)
        {
            try
            {
                for (var attempt = 0; ; attempt++)
                {
                    var r = await _httpClient.GetAsync($"v4/invoices/{requestId}");
                    var body = await r.Content.ReadAsStringAsync();

                    if (r.IsSuccessStatusCode) return (SdcLookup.Found, body);
                    if (r.StatusCode == System.Net.HttpStatusCode.NotFound) return (SdcLookup.NotFound, null);
                    if (attempt == 0 && body.Contains("1500")) { await SendPinAsync(); continue; } // PIN required
                    return (SdcLookup.Unavailable, null);
                }
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(ex, "SDC lookup failed for {RequestId}", requestId);
                return (SdcLookup.Unavailable, null);
            }
        }
    }
}
