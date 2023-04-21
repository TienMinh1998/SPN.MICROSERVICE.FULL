using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int fk_userid { get; set; }
        public int category_id { get; set; }
        public string questionname { get; set; }
        public string answer { get; set; }
        public string ImageSource { get; set; }
        public string phonetic { get; set; }
        public string audio { get; set; }
        public DateTime created_on { get; set; }
        public int is_delete { get; set; }
        public string definition { get; set; }
        public string Type { get; set; }
        public string Synonym { get; set; }   // từ đồng nghĩa
        public string Note { get; set; }      // Ghi chú
    }
}
