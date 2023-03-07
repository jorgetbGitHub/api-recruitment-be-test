using ApiApplication.Core;
using ApiApplication.Database.Entities;
using IMDbApiLib;
using IMDbApiLib.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class IMDbUpdaterHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<IMDbUpdaterHostedService> _logger;
        private readonly AppSettings _settings;
        private Timer _timer;

        public IMDbUpdaterHostedService(ILogger<IMDbUpdaterHostedService> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _settings = appSettings.Value;
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

        private async void DoWork(object state)
        {
            // Update IMDb Status [It works but is commented to do not exceed maximum api calls]
            var status = IMDBStatus.Instance;
            var apiLib = new ApiLib(_settings.IMDbApiKey);
            UsageData usage = await apiLib.UsageAsync();
            status.Up = usage.Count < usage.Maximum;
            status.LastCall = DateTime.Now;

            _logger.LogInformation($"IMDb Status successfully updated. LastCall: {status.LastCall}");
        }
    }
}
