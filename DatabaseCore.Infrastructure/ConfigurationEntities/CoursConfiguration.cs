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
    internal class CoursConfiguration : IEntityTypeConfiguration<Cours>
    {
        public void Configure(EntityTypeBuilder<Cours> builder)
        {
            builder.ToTable("Cours", schema: "usr");
        }
    }
}
