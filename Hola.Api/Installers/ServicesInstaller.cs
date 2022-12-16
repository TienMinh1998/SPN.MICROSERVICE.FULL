using Hola.Api.AutoMappers;
using Hola.Api.Repositories;
using Hola.Api.Repositories.CoursRepo;
using Hola.Api.Repositories.Grammar;
using Hola.Api.Repositories.QuestionrRepo;
using Hola.Api.Repositories.TargetRepo;
using Hola.Api.Repositories.UserManualRepo;
using Hola.Api.Repositories.UserRepository;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.CoursServices;
using Hola.Api.Service.GrammarServices;
using Hola.Api.Service.TargetServices;
using Hola.Api.Service.UserManualServices;
using Hola.Api.Service.UserServices;
using Hola.Api.Service.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Hola.Api.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapperSetup();
            services.AddTransient<Hola.Api.Service.QuestionService>();
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

            // Cours
            services.AddScoped<ICoursRepository, CoursRepository>();
            services.AddScoped<ICoursService, CoursService>();

            // UserManual
            services.AddScoped<IUserManualRepository, UserManualRepository>();
            services.AddScoped<IUserManualService, UserManualService>();

            // Question
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionService, Hola.Api.Service.V1.QuestionService>();
        }
    }
}
