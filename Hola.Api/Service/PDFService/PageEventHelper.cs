using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace Hola.Api.Service.PDFService
{
    public class PageEventHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = document.PageNumber;
            String text = "         Trang" + pageN + " of Report";
            float len = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED).GetWidthPoint(text, 8);
            Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(100, 100, 100);
            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED), 8);
            cb.SetTextMatrix(pageSize.GetRight(len + 10), pageSize.GetBottom(10));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetRight(len + 10), pageSize.GetBottom(10));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED), 8);
            template.SetTextMatrix(0, 0);
            template.EndText();
        }

    }
}
