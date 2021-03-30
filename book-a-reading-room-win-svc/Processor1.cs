using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace book_a_reading_room_win_svc
{
    internal class Processor1 : BackgroundService
    {
        private readonly ILogger<Processor1> _logger;

        public Processor1(ILogger<Processor1> logger)
        {
            _logger = logger;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            Console.WriteLine("Service Starting");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            Console.WriteLine("Service Stopping");
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            stoppingToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            await tcs.Task;

            _logger.LogInformation("Service stopped");
        }
    }
}
