using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;
using System.Text;

namespace ERS_Intergration.Services
{
    public class ApiKeyEntry
    {
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public string SourceSystem { get; set; } = ""; // "EMA", "MACMOBILE" or "*" (admin)
    }

    public class ApiKeyFilter : IAsyncAuthorizationFilter
    {
        public const string HeaderName = "X-Api-Key";
        public const string ItemKey = "ApiCaller";

        private readonly List<(ApiKeyEntry Entry, byte[] Hash)> _keys;

        public ApiKeyFilter(IConfiguration cfg)
        {
            var entries = cfg.GetSection("ApiKeys").Get<List<ApiKeyEntry>>() ?? new();
            _keys = entries
                .Where(e => !string.IsNullOrWhiteSpace(e.Key))
                .Select(e => (e, SHA256.HashData(Encoding.UTF8.GetBytes(e.Key))))
                .ToList();
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext ctx)
        {
            // Fail closed: no keys configured means nobody gets in
            if (!ctx.HttpContext.Request.Headers.TryGetValue(HeaderName, out var provided)
                || string.IsNullOrWhiteSpace(provided))
            {
                ctx.Result = new UnauthorizedObjectResult(new { error = "API key required." });
                return Task.CompletedTask;
            }

            var providedHash = SHA256.HashData(Encoding.UTF8.GetBytes(provided.ToString()));

            ApiKeyEntry? match = null;
            foreach (var (entry, hash) in _keys)
                if (CryptographicOperations.FixedTimeEquals(hash, providedHash))
                    match = entry; // no early break, keeps timing flat

            if (match == null)
            {
                ctx.Result = new UnauthorizedObjectResult(new { error = "Invalid API key." });
                return Task.CompletedTask;
            }

            ctx.HttpContext.Items[ItemKey] = match;
            return Task.CompletedTask;
        }
    }
}
