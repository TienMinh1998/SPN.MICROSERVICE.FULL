using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Category
    {
        public int Id { get; set; }
        public int fk_userid { get; set; }
        public string name { get; set; }
        public string define { get; set; }
        public string Image { get; set; }
        public int totalquestion { get; set; }
        public int priority { get; set; }
        public DateTime created_on { get; set; }
    }
}
