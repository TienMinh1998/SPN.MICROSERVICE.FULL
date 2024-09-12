using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Grammar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PK_grammarId { get; set; }
        public int FK_UserId { get; set; }
        public string grammar_name { get; set; }
        public string grammar_content { get; set; }
        public string Concept { get; set; }
        public DateTime created_on { get; set; }
        public string KD { get; set; }
        public string PD { get; set; }
        public string NV { get; set; }
        public string Code { get; set; }
    }
}
