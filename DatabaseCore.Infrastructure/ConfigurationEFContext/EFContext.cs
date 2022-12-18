using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Infrastructure.ConfigurationEFContext
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
        
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
            base.OnModelCreating(modelBuilder);
        }
    }
}
