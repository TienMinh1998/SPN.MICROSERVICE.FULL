using Hola.Api.Service.PDFService.Bases;
using System;
using System.Collections.Generic;

namespace Hola.Api.Service.PDFService.Models
{
    public class Pager
    {
        public List<TextItem> Header { get; set; }
        public List<IBody> Body { get; set; }
        public List<TextItem> Footter { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string MainTitle { get; set; }
        public ORIENTED ORIENTED_PAPER { get; set; }
    }

    public enum ORIENTED
    {
        Horizontal = 0,
        vertical
    }
}