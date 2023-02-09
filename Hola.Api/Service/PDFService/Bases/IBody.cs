using Hola.Api.Service.PDFService.Models;
using iTextSharp.text;
using System;

namespace Hola.Api.Service.PDFService.Bases
{
    public interface IBody
    {
        BODY_ITEMTYPE GetType();
        BaseColor GetHeaderColor();
    }
}
