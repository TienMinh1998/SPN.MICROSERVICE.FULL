using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hola.Core.Helper;
using Hola.Core.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hola.Core.Helper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<SettingModel> _config;

        public Worker(ILogger<Worker> logger,IOptions<SettingModel> _setting)
        {
            _logger = logger;
            _config = _setting;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker start running Deal service 'transaction/expire' request");
                try {
                    APICrossHelper aPICrossHelper = new APICrossHelper();
                    var rs = await aPICrossHelper.Get<JsonResponseModel>(_config.Value.TransferServiceUrl + "/Transaction/expire");
                    
                   
                    if (rs != null && rs.Status == 200)
                        _logger.LogInformation("transaction/expire response apistatus {stat}, done count {cnt}", rs.Status, rs.Data == null ? 0 : rs.Data);
                    else
                        _logger.LogInformation("'transaction/expire' response is null");


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception");
                }

                await Task.Delay(60000, stoppingToken);
            }
        
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync");

            await base.StartAsync(cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync");
            await base.StopAsync(cancellationToken);
        }
        
    }
}