using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using PdfSharp.Pdf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Hola.Api.Service;
using System.Collections.Generic;
using System.Linq;
using Hola.Api.Service.ExcelServices.TestModels;
using Newtonsoft.Json;
using Hola.Api.Service.PDFService;
using Hola.Api.Service.PDFService112;
using System.Reflection;

namespace Hola.Api.Controllers
{
    public class OpenAIController : ControllerBase
    {
        private string linksFont;
        private BaseFont baseFont;
        private PDFService service = new PDFService();
        private Font font12;
        private Font font10;
        private Font font9;
        private Font font12_bold;
        private Font whiteFont;


        public OpenAIController()
        {
            EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            SetFont("arial");
        }


        [HttpGet]
        [Route("GenatePDF")]
        public IActionResult GeneratePdf()
        {
            try
            {
                Document document = new Document();
                using (var stream = new MemoryStream())
                {

                    PageEventHelper pageEventHelper = new PageEventHelper();
                    var writer = PdfWriter.GetInstance(document, stream);
                    writer.PageEvent = pageEventHelper;
                    document.Open();

                    var table = SetHeader(font12, 200f, 300f);
                    var listsContent = BodyItem(new List<string>
                    {
                        "1. Họ và tên     : Nguyễn Viết Minh Tiến",
                        "2. Mã học viên   : 123456789.999",
                        "3. Ngày sinh     : 10-03-1999",
                        "4. Mã khóa học   : KH.99912",
                        "5. Hạng đào tạo  : B2",
                        "6. Cơ sở đào tạo : Trung tâm giáo dục nghề nghiệp và sát hạch lái xe Hà Nội"
                    },
                   "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmFowc1FYog7leGBz-9gdKO8n4cmlwXMqzkA&usqp=CAU");

                    // Step 8: Add the table to the document
                    Paragraph text1 = new Paragraph("Thái Bình, ngày 7 tháng 2 năm 2023", font9);
                    text1.Alignment = Element.ALIGN_RIGHT;
                    Paragraph subtitle = new Paragraph("(Ngày báo cáo : 11.01.2023)", font9);
                    subtitle.Alignment = Element.ALIGN_CENTER;

                    document.Add(table);
                    document.Add(text1);
                    document.Add(MainTitle("BÁO CÁO CHI TIẾT CỦA KHÓA HỌC THỰC HÀNH LÁI XE"));
                    document.Add(subtitle);
                    listsContent.ForEach(x => document.Add(x));
                    document.Add(Title("II. THÔNG TIN QUÁ TRÌNH ĐÀO TẠO"));
                    document.Add(WhiteSpace());

                    var list = new List<Student>();
                    for (int i = 0; i < 100000; i++)
                    {
                        var student = new Student
                        {
                            Vicle = "Vicle",
                            LoginTime = "LT17.11.2022",
                            LogoutTime = "LO 07.02.2023",
                            Session = "Session 001",
                            STT = i,
                            Time = "Time TEST",
                            url = "my url",
                        };
                        list.Add(student);
                    }
                    document.Add(SetTable(list, new List<HeaderConfig>
                    {
                        new HeaderConfig
                        {
                            ColumnName = "STT",
                            DisplayColumnName = "STT",
                            Width = 100,
                            ordinalNumber = 2,
                        },
                        new HeaderConfig
                        {
                            ColumnName = "Session",
                            DisplayColumnName = "Phiên học",
                            Width = 100,
                            ordinalNumber = 1,

                        },
                        new HeaderConfig
                        {
                            ColumnName = "Vicle",
                            DisplayColumnName = "Phương Tiện",
                            Width = 100,
                            ordinalNumber = 3,
                        },
                        new HeaderConfig
                        {
                            ColumnName = "LoginTime",
                            DisplayColumnName = "Thời gian đăng nhập",
                            Width = 100,
                            ordinalNumber = 4
                        },
                         new HeaderConfig
                        {
                            ColumnName = "LogoutTime",
                            DisplayColumnName = "Thời gian đăng xuất",
                            Width = 100,
                            ordinalNumber = 5,
                        },
                           new HeaderConfig
                        {
                            ColumnName = "Time",
                            DisplayColumnName = "Thời gian",
                            Width = 100,
                            ordinalNumber = 6,
                        },
                             new HeaderConfig
                        {
                            ColumnName = "url",
                            DisplayColumnName = "Ảnh",
                            Width = 100,
                            ordinalNumber = 7
                        }
                    })); ;
                    document.Add(WhiteSpace());
                    document.Add(SetFooter(font10, 250f, 250f));
                    // Step 9: Close the document
                    document.Close();
                    byte[] pdfBytes = stream.ToArray();

                    return File(pdfBytes, "application/pdf", "Example.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.ToString());
            }
            return null;
        }
        public PdfPTable SetHeader(Font font, float leftWidth, float rightWidth)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] widths = new float[] { leftWidth, rightWidth };
            table.SetWidths(widths);

            table.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cell1 = new PdfPCell(new Phrase("Sở giao thông vận tải Thái Bình \n Trung tâm Bình Anh \n ----------", font));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.Border = Rectangle.NO_BORDER;

            PdfPCell cellRight = new PdfPCell(new Phrase("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM \n Độc lập - Tự do - Hạnh phúc \n ----------", font));
            cellRight.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRight.Border = Rectangle.NO_BORDER;


            table.AddCell(cell1);
            table.AddCell(cellRight);

            return table;
        }

        public PdfPTable SetFooter(Font font, float leftWidth, float rightWidth)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.TotalWidth = 500f;
            table.LockedWidth = true;
            float[] widths = new float[] { leftWidth, rightWidth };
            table.SetWidths(widths);

            table.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cell1 = new PdfPCell(new Phrase("XÁC NHẬN CỦA HỌC VIÊN \n (Kí rõ họ tên)", font));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.Border = Rectangle.NO_BORDER;

            PdfPCell cellRight = new PdfPCell(new Phrase("Thái bình, ngày 7 tháng 2 năm 2023 \n GIÁM ĐỐC \n\n\n\n\n\n Nguyễn Viết Minh Tiến", font));
            cellRight.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRight.Border = Rectangle.NO_BORDER;


            table.AddCell(cell1);
            table.AddCell(cellRight);

            return table;
        }

        public List<IElement> BodyItem(List<string> listContent, string urlImage)
        {
            try
            {
                List<IElement> elements = new List<IElement>();
                string content = "";
                foreach (var item in listContent)
                {
                    content += "\n " + item;
                }

                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 400f, 100f };
                table.SetWidths(widths);
                table.DefaultCell.Border = Rectangle.BOX;

                PdfPCell cell1 = new PdfPCell(new Phrase(content, font10));
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.Padding = 4f;

                // image 
                var image = GetImageFromUrl(urlImage);
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
            font12_bold = new Font(baseFont, 12, Font.BOLD);
            whiteFont = new Font(baseFont, 12, Font.BOLD);
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
        public PdfPTable SetTable<T>(List<T> collection, List<HeaderConfig> listHeaderConfig)
        {
            // Xử lý Config
            Type modelType = typeof(T);
            var properties = typeof(T).GetProperties();
            var sortedList = listHeaderConfig.OrderBy(x => x.ordinalNumber).ToList();
            List<PropertyInfo> sortedProperties = new List<PropertyInfo> { };
            foreach (var item in sortedList)
            {
                var value = properties.FirstOrDefault(x => x.Name.Trim() == item.ColumnName);
                if (value != null)
                {
                    sortedProperties.Add(value);
                }
                else
                {
                    sortedProperties.Add(null);
                }
            }

            int count = properties.Count();
            PdfPTable table = new PdfPTable(count);
            table.TotalWidth = 500f;
            table.LockedWidth = true;


            foreach (var item in sortedProperties)
            {
                var columnName = listHeaderConfig.FirstOrDefault(x => x.ColumnName.Trim() == item.Name.Trim());
                if (columnName != null)
                {
                    var displayName = columnName.DisplayColumnName;
                    PdfPCell cell = new PdfPCell(new Phrase(displayName, whiteFont));
                    cell.BackgroundColor = BaseColor.GRAY;

                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                else
                {
                    table.AddCell("undefine");
                }
            }

            foreach (var item in collection)
            {
                foreach (var p in sortedProperties)
                {
                    var name = p.Name;
                    var value = p.GetValue(item, null);

                    PdfPCell cell = new PdfPCell(new Phrase(value.ToString(), font9));
                    if (value != null)
                    {
                        table.AddCell(cell);
                    }
                    else
                    {
                        table.AddCell("");
                    }
                }
            }


            return table;
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
    }
}
