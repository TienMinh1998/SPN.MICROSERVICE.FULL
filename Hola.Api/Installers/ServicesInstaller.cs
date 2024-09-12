using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.AutoMappers;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.CateporyServices;
using Hola.Api.Service.CoursServices;
using Hola.Api.Service.CoursServices.V1;
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
            services.AddTransient<Service.QuestionService>();
            services.AddSingleton<Service.CategoryService>();
            services.AddTransient<AccountService>();
            services.AddTransient<FirebaseService>();
            services.AddScoped<DapperBaseService>();
            // Target Service
            services.AddScoped<ITargetService, TargetService>();

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


            // Category
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, Service.CateporyServices.v1.CategoryService>();

            // Question
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionService, Hola.Api.Service.V1.QuestionService>();

            // Notification
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<INotificationService, NotificationService>();

            // Topic
            services.AddTransient<ITopicRepository, TopicRepository>();
            services.AddTransient<ITopicService, TopicService>();

            // QuestionStandard



            // Reading
            services.AddTransient<IReadingRepository, ReadingRepository>();
            services.AddTransient<IReadingService, ReadingService>();

            // UPload Service 
            services.AddTransient<IUploadFileService, UploadService>();

            services.AddTransient<IPhraseRepository, PhraseRepository>();
            services.AddTransient<IPhraseService, PhraseService>();
            // Report 
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IReportService, ReportService>();
            // News 
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<INewsService, NewsService>();
            // Product 
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
        }
    }
}
