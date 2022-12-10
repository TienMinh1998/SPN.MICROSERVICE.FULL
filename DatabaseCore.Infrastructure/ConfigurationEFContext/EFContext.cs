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
        public DbSet<User> Users { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Grammar> Grammars { get; set; }

        public DbSet<Cours> Cours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Config table in postgressSQl
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TargetConfiguration());
            modelBuilder.ApplyConfiguration(new GrammarConfiguration());
            modelBuilder.ApplyConfiguration(new CoursConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
