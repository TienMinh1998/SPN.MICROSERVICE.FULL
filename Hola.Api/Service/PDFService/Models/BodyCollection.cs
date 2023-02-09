using Hola.Api.Service.PDFService.Bases;
using iTextSharp.text;
using System;



namespace Hola.Api.Service.PDFService.Models
{
    public class BodyCollection : BaseBody, IBody
    {
        public TableConfig Collection { get; set; }
        public BaseColor GetHeaderColor()
        {
            return Collection.HeaderColor;
        }
        BODY_ITEMTYPE IBody.GetType()
        {
            return this.TYPE;
        }
    }
}
