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
    public class QuestionStandardDetailConfiguration : IEntityTypeConfiguration<QuestionStandardDetail>
    {
        public void Configure(EntityTypeBuilder<QuestionStandardDetail> builder)
        {
            builder.ToTable("QuestionStandardDetail", schema: "usr");
            builder.HasKey(x => new { x.QuestionID, x.TopicID }).HasName("PrimaryKey_QuestionStandardAndTopic");
        }
    }
}
