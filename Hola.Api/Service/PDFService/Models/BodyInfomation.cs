using Hola.Api.Service.PDFService.Bases;
using iTextSharp.text;
using System.Collections.Generic;

namespace Hola.Api.Service.PDFService.Models
{
    public class BodyInfomation : BaseBody, IBody
    {
        public string UrlImage { get; set; }
        public List<string> Texts { get; set; }

        public BaseColor GetHeaderColor()
        {
            return new BaseColor(0, 191, 255);
        }

        BODY_ITEMTYPE IBody.GetType()
        {
            return this.TYPE;
        }
    }
}
