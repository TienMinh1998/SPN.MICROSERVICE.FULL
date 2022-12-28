using System;
using System.Collections.Generic;

namespace Hola.Api.Requests
{
    public class QuestionModelStandard
    {
        public string searchKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string columnname { get; set; }
        public string Date { get; set; }
        public bool IsDesc { get; set; }
    }
}
