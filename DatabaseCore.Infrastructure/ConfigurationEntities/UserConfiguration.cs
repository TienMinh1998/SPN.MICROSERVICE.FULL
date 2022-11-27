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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", schema: "usr");
            builder.Property(x => x.PhoneNumber).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.Username).HasColumnType("varchar").HasMaxLength(50);
        }
    }
}
