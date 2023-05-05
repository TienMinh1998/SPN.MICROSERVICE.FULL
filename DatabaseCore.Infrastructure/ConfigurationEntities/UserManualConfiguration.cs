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
    public class UserManualConfiguration : IEntityTypeConfiguration<UserManual>
    {
        public void Configure(EntityTypeBuilder<UserManual> builder)
        {
            builder.ToTable("UserManual", schema: "usr");

        }
    }
}
