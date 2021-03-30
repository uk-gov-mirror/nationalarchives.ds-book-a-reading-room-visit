using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Threading.Tasks;

namespace book_a_reading_room_win_svc
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, CommandLineOptions opts) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(configureLogging => configureLogging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton(opts);
                        services.AddHostedService<Processor1>()
                            .Configure<EventLogSettings>(config =>
                            {
                                config.LogName = "Processor 1";
                                config.SourceName = "Processor 1 Source";
                            });
                    }).UseWindowsService();
    }
}
