using DatabaseCore.Domain.Entities.Normals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Infrastructure.ConfigurationEntities
{
    public class TargetConfiguration : IEntityTypeConfiguration<Target>
    {
        public void Configure(EntityTypeBuilder<Target> builder)
        {
            builder.ToTable("Target", schema: "usr");
            builder.Property(x => x.created_on).HasColumnType("timestamp");
            builder.Property(x => x.start_date).HasColumnType("timestamp");
            builder.Property(x => x.end_date).HasColumnType("timestamp");
        }
    }
}
