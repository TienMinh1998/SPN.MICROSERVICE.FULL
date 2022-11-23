
using Hola.Api.Service;
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
            services.AddScoped<IUploadFileGoogleCloudStorage, HolaGoogleStorage>();
        }
    }
}
