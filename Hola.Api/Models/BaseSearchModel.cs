using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class BaseSearchModel
    {
        public virtual int PageIndex { get; set; }
        public virtual int PageSize { get; set; }
        public virtual Dictionary<string, string> Search { get; set; }
    }
}
