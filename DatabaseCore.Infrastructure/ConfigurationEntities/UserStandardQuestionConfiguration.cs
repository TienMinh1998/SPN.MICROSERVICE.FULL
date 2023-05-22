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
    public class UserStandardQuestionConfiguration : IEntityTypeConfiguration<UserStandardQuestion>
    {
        public void Configure(EntityTypeBuilder<UserStandardQuestion> builder)
        {
            builder.ToTable("UserStandardQuestion", schema: "usr");
            builder.HasKey(x => new { x.StandardQuestion, x.UserId }).HasName("PrimaryKey_QuestionStandardAndUser");
        }
    }
}
