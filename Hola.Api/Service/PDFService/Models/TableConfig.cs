using iTextSharp.text;
using System.Collections.Generic;

namespace Hola.Api.Service.PDFService.Models
{
    public class TableConfig
    {
        public List<HeaderConfig> HeaderConfig { get; set; }
        public List<FootterItem> FootterConfig { get; set; }
        public List<object> Data { get; set; }
        public BaseColor HeaderColor { get; set; }
    }
}
