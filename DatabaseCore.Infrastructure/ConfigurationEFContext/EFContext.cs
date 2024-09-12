using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Domain.Questions;
using DatabaseCore.Infrastructure.ConfigurationEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseCore.Infrastructure.ConfigurationEFContext
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options, IMediator mediator) : base(options)
        {
            this.mediator = mediator;
        }
        // Define table
        public DbSet<User> Users { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<Cours> Cours { get; set; }
        public DbSet<UserManual> UserManuals { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<QuestionStandard> QuestionStandards { get; set; }
        public DbSet<QuestionStandardDetail> questionStandardDetails { get; set; }
        public DbSet<UserStandardQuestion> UserStandardQuestions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        // bảng reading
        public DbSet<Reading> Readings { get; set; }
        public DbSet<ReadingQuestion> ReadingsQuestions { get; set; }
        public DbSet<Phrase> Phrases { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Config table in postgressSQl
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TargetConfiguration());
            modelBuilder.ApplyConfiguration(new GrammarConfiguration());
            modelBuilder.ApplyConfiguration(new CoursConfiguration());
            modelBuilder.ApplyConfiguration(new UserManualConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new TopicConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionStandardDetailConfiguration());
            modelBuilder.ApplyConfiguration(new UserStandardQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new PhraseConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        private IMediator mediator;

        public async Task<bool> SaveEntityAsync(CancellationToken cancellationToken = default)
        {
            await mediator.DispatDomain(this);
            _ = await base.SaveChangesAsync();
            return true;
        }

    }
}
