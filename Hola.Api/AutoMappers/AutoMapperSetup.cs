using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hola.Api.AutoMappers
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(
                 typeof(RequestToEntityProfile)
                );
            AutoMapperConfig.RegisterMappings();
        }
    }
}
