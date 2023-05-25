using DatabaseCore.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals;

public class Reading : BaseEntity<int>
{
    public string Title { get; set; }
    public string Definetion { get; set; }
    public string Image { get; set; }
    public string Content { get; set; }
    public string Translate { get; set; }
    public string Status { get; set; }
    public string TaskName { get; set; }
    public int Band { get; set; }
}
