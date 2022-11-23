using Hola.Api.Configurations;
using Hola.Api.Service;
using Hola.Api.Service.FilterExtension;
using Hola.Api.Service.UploadFile;
using Hola.GoogleCloudStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


namespace Hola.Api.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Bind Configuration
            var redisConfiguration = new RedisConfiguration();
            configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
            services.AddSingleton(redisConfiguration);
            if (!redisConfiguration.Enabled) return;
            services.AddSingleton<IConnectionMultiplexer>(_=>ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
            services.AddStackExchangeRedisCache(option=>option.Configuration = redisConfiguration.ConnectionString);
            services.AddSingleton<IResponseCacheService,ResponseCacheService>();
            services.AddTransient<IUploadFileService, UploadFileService>();
            services.AddTransient<ISearchFilterService, SearchFilterService>();
            services.AddScoped<IUploadFileGoogleCloudStorage, HolaGoogleStorage>();
        }
    }
}
