
/*---------------------------------|
| Author     :  Criss --------------|
| CreateDate :  2022-12-19 ---------|
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
                .WithIdentity(jobName + "-trigger").WithCronSchedule(cronSchedule));
        }

        /// <summary>
        /// Task Thông báo 1 10 phút 1 lần
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="quartz"></param>
        /// <param name="config"></param>
        /// <exception cref="Exception"></exception>
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
                .WithIdentity(jobName + "-trigger").WithCronSchedule(cronSchedule));
        }

    }
}
