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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
                builder.ToTable("question", schema: "usr");
                builder.Property(x => x.created_on).HasColumnType("timestamp"); // không thấy timezone
        }
    }
}
