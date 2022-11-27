using Hola.Api.Repositories;
using Hola.Api.Repositories.UserRepository;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.UserServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Hola.Api.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<QuestionService>();
            services.AddSingleton<CategoryService>();
            services.AddTransient<AccountService>();
            services.AddTransient<FirebaseService>();
            // add baseService and BaseRepository
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            // User
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

        }
    }
}
