
using Hola.Api.Service.ExcelServices.TestModels;
using Hola.Api.Service.PDFService.Models;
using Hola.Api.Service.PDFService;
using iTextSharp.text.pdf;
using iTextSharp.text;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Reflection;
using System.Text;
using Hola.Api.Service.PDFService.Bases;

namespace Hola.Api.Service.PDFService112;

public class PDFService
{
    private string linksFont;
    private BaseFont baseFont;
    private Font font12;
    private Font font8;
    private Font font10;
    private Font font9;
    private Font font9_bold;
    private Font font12_bold;
    private Font whiteFont;
    public PDFService()
    {
        EncodingProvider ppp = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(ppp);
        SetFont("arial");
    }

    public Font GetFont(int value)
    {
        Font font = new Font();
        try
        {
            switch (value)
            {
                case 8:
                    font = font9;
                    break;
                case 9:
                    font = font9_bold;
                    break;
                case 10:
                    font = font10;
                    break;
                case 11:
                    font = font12;
                    break;
                case 12:
                    font = font12;
                    break;
                default:
                    font = font9;
                    break;
            }
            return font;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public byte[] GenratePDF(Pager paperConfig)
    {
        // Tạo tài liệu
        string stringDay = $"Thái Bình, Ngày {paperConfig.CreateDate.Day} tháng {paperConfig.CreateDate.Month} năm {paperConfig.CreateDate.Year}";
        string title = paperConfig.MainTitle;
        string subTitle = $"(Ngày báo cáo : {paperConfig.CreateDate.ToString("dd-MM-yyyy")})";

        var elements = paperConfig.Body;
        var headers = paperConfig.Header;
        float tableWidth;

        try
        {
            // Cài đặt chiều của khổ giấy
            Rectangle page = PageSize.A4;
            if (paperConfig.ORIENTED_PAPER == ORIENTED.Horizontal)
            {
                tableWidth = 750f;
                page = PageSize.A4.Rotate();
            }
            else
            {
                tableWidth = 500f;
            }


            Document document = new Document(page);
            using (var stream = new MemoryStream())
            {
                PageEventHelper pageEventHelper = new PageEventHelper();
                var writer = PdfWriter.GetInstance(document, stream);
                writer.PageEvent = pageEventHelper;
                document.Open();
                var tableHeaderhaveImage = CreateNewInfomationAndImage((BodyInfomation)elements.First(), tableWidth);
                var headerOfPaper = BuildHeaderElement(headers, stringDay, title, subTitle, 2f, 3f, tableWidth);
                headerOfPaper.ForEach(hd => document.Add(hd));

                foreach (var bodyElement in elements)
                {
                    if (bodyElement.GetType() == BODY_ITEMTYPE.TEXT_AND_IMAGE)
                    {
                        tableHeaderhaveImage.ForEach(x => document.Add(x));
                    }
                    else
                    {
                        var bodyCollection = (BodyCollection)bodyElement;
                        Type myType = bodyCollection.GetType();

                        var list1 = bodyCollection.Collection.Data;
                        document.Add(Title(bodyCollection.Title));
                        document.Add(WhiteSpace());
                        document.Add(SetCollection(bodyCollection.Collection, tableWidth));
                        document.Add(WhiteSpace());
                    }
                }
                document.Add(SetFooter(font10, 250f, 250f, paperConfig.Footter));

                // Step 9: Close the document
                document.Close();
                byte[] pdfBytes = stream.ToArray();
                return pdfBytes;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error" + ex.ToString());
        }
        return null;
    }


    public List<IElement> BuildHeaderElement(List<TextItem> listValue, string strDay, string title, string _subtitle, float leftSpace, float RightSpace, float tableWidth)
    {
        try
        {
            List<IElement> headers = new List<IElement>();
            var table = SetHeader(leftSpace, RightSpace, listValue, tableWidth);
            Paragraph dayOfReport = new Paragraph(strDay, font9);
            dayOfReport.Alignment = Element.ALIGN_RIGHT;
            var element3 = MainTitle(title);
            Paragraph subtitle = new Paragraph(_subtitle, font9);
            subtitle.Alignment = Element.ALIGN_CENTER;
            headers.Add(table);
            headers.Add(dayOfReport);
            headers.Add(element3);
            headers.Add(subtitle);
            return headers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
            return new List<IElement> { };
        }
    }

    public PdfPTable SetHeader(float spaceLeft, float spaceRight, List<TextItem> listHeaderValue, float tableWidth)
    {
        try
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.TotalWidth = tableWidth;
            table.LockedWidth = true;
            float[] widths = new float[] { spaceLeft, spaceRight };
            table.SetWidths(widths);
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            foreach (var item in listHeaderValue)
            {
                Font font = new Font();
                if (item.FontStyle != null)
                {
                    font = item.FontStyle;
                }
                else
                {
                    font = font10;
                }
                PdfPCell cell = new PdfPCell(new Phrase(item.Value, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = Rectangle.NO_BORDER;

                table.AddCell(cell);
            }
            return table;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex.Message);
            return default(PdfPTable);
        }
    }
    public PdfPTable SetFooter(Font font, float leftWidth, float rightWidth, List<TextItem> listFootter)
    {
        try
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] widths = new float[] { leftWidth, rightWidth };
            table.SetWidths(widths);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            foreach (var item in listFootter)
            {
                PdfPCell cell = new PdfPCell(new Phrase(item.Value, font));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);
            }
            return table;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error" + ex.Message);
            return default(PdfPTable);
        }
    }
    public List<IElement> CreateNewInfomationAndImage(BodyInfomation body, float tableWidth)
    {
        try
        {
            List<IElement> elements = new List<IElement>();
            string content = "";
            foreach (var item in body.Texts)
            {
                content += "\n " + item;
            }

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = tableWidth;
            table.LockedWidth = true;
            float[] widths;
            // 750 is width 
            if (tableWidth < 700f)
            {
                widths = new float[] { 400f, 100f };
            }
            else
            {
                widths = new float[] { 650f, 100f };
            }



            table.SetWidths(widths);
            table.DefaultCell.Border = Rectangle.BOX;

            PdfPCell cell1 = new PdfPCell(new Phrase(content, font10));
            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell1.Padding = 4f;

            // image 
            var image = GetImageFromUrl(body.UrlImage);
            var memoryStream = new MemoryStream(image);
            Image myImage = Image.GetInstance(memoryStream);
            myImage.ScaleAbsolute(10f, 10f);
            myImage.Alignment = Element.ALIGN_RIGHT;

            PdfPCell cell = new PdfPCell(myImage, true);
            cell.Padding = 2f;
            table.AddCell(cell1);
            table.AddCell(cell);

            elements.Add(Title("I. THÔNG TIN HỌC VIÊN"));
            elements.Add(WhiteSpace());
            elements.Add(table);
            return elements;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return new List<IElement>();
    }
    public void SetFont(string fontName)
    {
        linksFont = Path.Combine(Directory.GetCurrentDirectory(), $"PdfTemplate\\{fontName}.ttf");
        baseFont = BaseFont.CreateFont(linksFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        font12 = new Font(baseFont, 12, Font.NORMAL);
        font10 = new Font(baseFont, 10, Font.NORMAL);
        font9 = new Font(baseFont, 9, Font.NORMAL);
        font8 = new Font(baseFont, 8, Font.NORMAL);
        font9_bold = new Font(baseFont, 9, Font.BOLD);

        font12_bold = new Font(baseFont, 12, Font.BOLD);
        whiteFont = new Font(baseFont, 10, Font.BOLD);
        whiteFont.SetColor(255, 255, 255);
    }
    public Paragraph WhiteSpace()
    {
        Paragraph WhiteSpace = new Paragraph("\n", font9);
        return WhiteSpace;
    }
    public Paragraph Title(string title)
    {
        Paragraph infomation = new Paragraph(title, font12_bold);
        infomation.Alignment = Element.ALIGN_LEFT;
        return infomation;
    }
    public Paragraph MainTitle(string title)
    {
        Paragraph infomation = new Paragraph(title, font12_bold);
        infomation.Alignment = Element.ALIGN_CENTER;
        return infomation;
    }
    public PdfPTable SetCollection(TableConfig tableconf, float tableWidth)
    {
        try
        {
            // Khai báo list độ rộng của cột, tổng độ rộng của cột
            List<float> widthValues = new List<float>();
            float totalWidth = 0;

            // Lấy ra kiểu phân tử của Collection, danh sách các thuộc tính tương ứng của nó 
            Type modelType = GetItemTypeOfList(tableconf.Data);
            var properties = modelType.GetProperties();
            int count = properties.Count();
            int[] arrayAlign = new int[count];
            List<PropertyInfo> sortedProperties = new List<PropertyInfo> { };
            List<HeaderConfig> sortedList;

            // Kiểm tra xem header có null không, Nếu null thì lấy cấu hình mặc định
            // bằng cách gán giá trị mới cho nó 
            if (tableconf.HeaderConfig != null)
            {
                sortedList = tableconf.HeaderConfig.OrderBy(x => x.ordinalNumber).ToList();
            }
            else
            {
                int index = 0;
                sortedList = new List<HeaderConfig>();
                foreach (var item in properties)
                {
                    sortedList.Add(new HeaderConfig
                    {
                        Align = Element.ALIGN_LEFT,
                        ColumnName = item.Name,
                        DisplayColumnName = item.Name,
                        ordinalNumber = index + 1,
                        Width = 20f
                    });
                    ;
                }
            }




            // Cài đặt độ rộng của cột, ddoort

            int i = 0;
            foreach (var item in sortedList)
            {
                if (item.Width > 0)
                {
                    widthValues.Add(item.Width);
                    totalWidth += item.Width;
                }
                else
                {
                    widthValues.Add(15f);
                    totalWidth += 15f;
                }

                var value = properties.FirstOrDefault(x => x.Name.Trim() == item.ColumnName);
                if (value != null)
                {
                    sortedProperties.Add(value);
                }
                else
                {
                    sortedProperties.Add(null);
                }

                arrayAlign[i] = item.Align;
                i++;
            }
            PdfPTable table = new PdfPTable(count);
            table.TotalWidth = tableWidth;
            table.LockedWidth = true;
            table.SetWidths(widthValues.ToArray());
            foreach (var item in sortedProperties)
            {
                var columnName = sortedList.FirstOrDefault(x => x.ColumnName.Trim() == item.Name.Trim());
                if (columnName != null)
                {
                    var displayName = columnName.DisplayColumnName;
                    PdfPCell cell = new PdfPCell(new Phrase(displayName, whiteFont));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = tableconf.HeaderColor;
                    table.AddCell(cell);
                }
                else
                {
                    table.AddCell("Column");
                }
            }
            foreach (var item in tableconf.Data)
            {
                int columnIndex = 0;
                foreach (var p in sortedProperties)
                {
                    var name = p.Name;
                    var value = p.GetValue(item, null);
                    PdfPCell cell = new PdfPCell(new Phrase(value.ToString(), font9));
                    cell.HorizontalAlignment = arrayAlign[columnIndex];
                    if (cell != null)
                    {
                        table.AddCell(cell);
                    }
                    else
                    {
                        table.AddCell("");
                    }
                    columnIndex++;
                }
            }
            if (tableconf.FootterConfig.Count > 0)
                foreach (var footer in tableconf.FootterConfig)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(footer.Value, font10));
                    cell.Colspan = (footer.ColSpan > properties.Count()) ? properties.Count() : footer.ColSpan;
                    cell.HorizontalAlignment = footer.TextAlign;
                    table.AddCell(cell);
                }

            return table;
        }
        catch (Exception ex)
        {
            return default;
        }
    }
    public byte[] GetImageFromUrl(string url)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                return httpClient.GetByteArrayAsync(url).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading image from URL: " + url + ", error: " + ex.Message);
                return new byte[0];
            }
        }
    }
    public Type GetItemTypeOfList(List<object> myList)
    {
        if (myList == null || myList.Count == 0)
        {
            return null;
        }
        return myList.First().GetType();
    }

}
