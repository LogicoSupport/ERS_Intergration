namespace ERS_Intergration.Services
{
    public class PendingRecoveryWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopes;
        private readonly ILogger<PendingRecoveryWorker> _log;
        private readonly TimeSpan _interval, _minAge;

        public PendingRecoveryWorker(IServiceScopeFactory scopes, ILogger<PendingRecoveryWorker> log, IConfiguration cfg)
        {
            _scopes = scopes; _log = log;
            _interval = TimeSpan.FromSeconds(cfg.GetValue("Recovery:IntervalSeconds", 60));
            _minAge = TimeSpan.FromSeconds(cfg.GetValue("Recovery:MinAgeSeconds", 120));
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var timer = new PeriodicTimer(_interval);
            try
            {
                while (await timer.WaitForNextTickAsync(ct))
                {
                    try
                    {
                        using var scope = _scopes.CreateScope();
                        var svc = scope.ServiceProvider.GetRequiredService<FiscalisationService>();
                        var n = await svc.RecoverPendingAsync(_minAge, ct);
                        if (n > 0) _log.LogInformation("Recovered {Count} pending invoice(s)", n);
                    }
                    catch (Exception ex) when (!ct.IsCancellationRequested)
                    {
                        _log.LogError(ex, "Pending recovery sweep failed");
                    }
                }
            }
            catch (OperationCanceledException) { /* shutting down */ }
        }
    }
}
