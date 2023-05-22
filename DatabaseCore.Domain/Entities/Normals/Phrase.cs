using DatabaseCore.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Phrase : BaseEntity<int>
    {
        public int fk_readingId { get; set; }
        public string word { get; set; }
        public string definition { get; set; }
    }
}
