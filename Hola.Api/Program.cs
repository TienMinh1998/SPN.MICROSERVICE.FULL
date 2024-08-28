using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hola.Api.Service.Quatz;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Sentry;
using Sentry.Extensibility;

namespace Hola.Api
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        [Obsolete]
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Add the required Quartz.NET services
                services.AddQuartz(q =>
                {
                    // Use a Scoped container to create jobs. I'll touch on this later

                    q.AddJobAndTrigger<JobClass>(hostContext.Configuration);
                    q.AddJob<EveryDayNotificationClass>(hostContext.Configuration);
                    q.AddJobAndTrigger<HistoryEveryDayJob>(hostContext.Configuration);
                    q.AddJobAndTrigger<jobStanStandardQuestion>(hostContext.Configuration);
                });

                // Add the Quartz.NET hosted service

                services.AddQuartzHostedService(
                    q => q.WaitForJobsToComplete = true);

                // other config
            })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseSentry();
                   webBuilder.UseStartup<Startup>();
               });
    }
}