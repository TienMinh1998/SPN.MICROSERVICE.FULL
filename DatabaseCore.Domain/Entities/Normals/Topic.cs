using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    /// <summary>
    /// các chủ đề của khóa học
    /// </summary>
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PK_Topic_Id { get; set; }
        public int FK_Course_Id { get; set; }
        public string Image { get; set; }
        public string EnglishContent { get; set; }
        public string VietNamContent { get; set; }
        public DateTime created_on { get; set; }
    }
}
