using ApiApplication.Database.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class IMDbUpdaterHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<IMDbUpdaterHostedService> _logger;
        private Timer _timer;

        public IMDbUpdaterHostedService(ILogger<IMDbUpdaterHostedService> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("IMDb Updater Hosted Service running.");
            
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("IMDb Updater Hosted Service is stopping.");
            
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            // Update IMDb Status
            var status = IMDBStatus.Instance;
            status.Up = true;
            status.LastCall = DateTime.Now;

            _logger.LogInformation($"IMDb Status successfully updated. LastCall: {status.LastCall}");
        }
    }
}
