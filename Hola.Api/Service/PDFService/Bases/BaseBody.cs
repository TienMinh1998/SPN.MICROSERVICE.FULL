using Hola.Api.Service.PDFService.Models;

namespace Hola.Api.Service.PDFService.Bases
{
    public abstract class BaseBody
    {
        public string Title { get; set; }
        public BODY_ITEMTYPE TYPE { get; set; }
    }
}
