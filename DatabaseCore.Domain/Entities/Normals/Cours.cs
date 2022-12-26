using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Cours
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pk_coursId { get; set; }
        public string Code { get; set; }        // Mã khóa học
        public string Title { get; set; }
        public string Target { get; set; }
        public string Content { get; set; }
        public string CoursImage { get; set; }
        public DateTime created_on { get; set; }
    }
}
