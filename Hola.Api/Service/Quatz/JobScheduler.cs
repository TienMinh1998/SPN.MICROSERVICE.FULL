
/*---------------------------------|
| Author     :  Criss --------------|
| CreateDate :  11/27/2022 ---------|
| Content    :  Background Worker --|
|---------------------------------*/


using Microsoft.Extensions.Configuration;
using Quartz;
using System;

namespace Hola.Api.Service.Quatz
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config) where T : IJob
        {
            string jobName = typeof(T).Name;
            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];
            if (string.IsNullOrEmpty(cronSchedule))
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            // REGISTER JOB 
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger").WithCronSchedule("0 10 21 ? * *"));
               // .WithSimpleSchedule(s => s.WithIntervalInMinutes(3).RepeatForever()));
        }


        public static void AddJob<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config) where T : IJob
        {
            string jobName = typeof(T).Name;
            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];
            if (string.IsNullOrEmpty(cronSchedule))
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            // REGISTER JOB 
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithSimpleSchedule(s => s.WithIntervalInHours(24).RepeatForever()));
        }

    }
}
