using Hola.Api.AutoMappers;
using Hola.Api.Repositories;
using Hola.Api.Repositories.Grammar;
using Hola.Api.Repositories.TargetRepo;
using Hola.Api.Repositories.UserRepository;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.GrammarServices;
using Hola.Api.Service.TargetServices;
using Hola.Api.Service.UserServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Hola.Api.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapperSetup();
            services.AddTransient<QuestionService>();
            services.AddSingleton<CategoryService>();
            services.AddTransient<AccountService>();
            services.AddTransient<FirebaseService>();

            // Target Service
            services.AddScoped<ITargetService,TargetService>();

            // add baseService and BaseRepository
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            // User
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            // Grammar
            services.AddScoped<IGrammarRepository, GrammarRepository>();
            services.AddScoped<IGrammarService, GrammarService>();

            // Target
            services.AddScoped<ITargetRepository, TargetRepository>();
            services.AddScoped<ITargetService, TargetService>();

        }
    }
}
