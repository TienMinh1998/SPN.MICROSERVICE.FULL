
using Hola.Api.Service.ExcelServices.TestModels;
using Hola.Api.Service.IText7.DefaultConfig;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Hola.Api.Service.IText7
{
    public class LibPDfService
    {
        // Danh sách phần tử để thêm vào document 
        private IList<IBlockElement> blockElements;
        private Document document;
        private PdfFont currentFont;
        private List<HeaderConfig> headerFomats;
        public LibPDfService()
        {
            blockElements = new List<IBlockElement>();
        }
        public byte[] CreateDocument(InputPage input)
        {
            // Tạo tệp mới PDF

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                document = new Document(pdf);

                CreateNewFont("arial.ttf");
                CreateNewHeader(input.HeaderInput);

                foreach (var item in input.BodyModel.BodyItems)
                {
                    var myType = item.GetBodyType();
                    if (myType == BODY_TYPE.TEXT_AND_IMAGE)
                    {
                        CreateNewTableGridImfomationAndImage(item.GetStudentInfomation());
                    }
                    else
                    {
                        CreateContent(item.GetCollection(), item.GetTitle());
                    }
                }
                CreateNewFootter();
                CloseDocument();
                byte[] pdfBytes = stream.ToArray();
                return pdfBytes;
            }
        }
        public void CreateNewHeader(HeaderModel headerModel)
        {
            var datatime = DateTime.Now;
            CreateNewTable(headerModel.HeaderDefault, TextAlignment.CENTER, iText.Layout.Borders.Border.NO_BORDER);
            CreateNewText($"Hà nội, ngày {datatime.Day} tháng {datatime.Month} năm {datatime.Year}", 10, TextAlignment.RIGHT, 10, true, false);
            foreach (var main in headerModel.MainTitles)
                CreateNewText(main, 15, TextAlignment.CENTER, 10, true, true);
            foreach (var sub in headerModel.SubTitles)
                CreateNewText(sub, 10, TextAlignment.CENTER, 10, true, false);



        }
        public void CreateContent(List<object> data, string tableTitle)
        {
            CreateNewText(tableTitle, 12, TextAlignment.LEFT, 15, true, true);
            CreateNewTableCollection(data);
            CreateNewText(" ", 12, TextAlignment.LEFT, 15, true, true);
        }
        public void CreateNewFont(string fontName)
        {
            var fontPath = Path.Combine(Directory.GetCurrentDirectory(), $"PdfTemplate/{fontName}");
            currentFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
        }
        public Table CreateNewTableGridImfomationAndImage(string content)
        {
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(new float[] { 500, 100 });
            table.SetPadding(IText7Param.PADING_DEFAULT);
            table.SetWidth(UnitValue.CreatePercentValue(IText7Param.PERCENT_OF_TABLE_DEFAULT));
            var text = CreateNewText(content, false, false, 10);
            var image = CreatePdfImage("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmFowc1FYog7leGBz-9gdKO8n4cmlwXMqzkA&usqp=CAU");

            float width = Convert.ToSingle(IText7Param.IMAGE_WID_DEFAULT);
            float height = Convert.ToSingle(IText7Param.IMAGE_HEIGH_DEFAULT);
            image.ScaleAbsolute(width, height);
            image.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            Cell cell = new Cell();
            cell.Add(text);

            Cell cellImage = new Cell();
            cellImage.Add(image);
            table.AddCell(cell);
            table.AddCell(cellImage);
            blockElements.Add(table);
            return table;
        }
        public Table CreateNewTable(List<string> cells, TextAlignment alignment, iText.Layout.Borders.Border border)
        {
            Table table = new Table(new float[] { 150, 150 });
            table.SetPadding(0);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            foreach (var content in cells)
            {
                var text = CreateNewText(content, false, false, 10);
                text.SetPadding(0f);
                Cell cell = new Cell();
                cell.Add(text);
                cell.SetTextAlignment(alignment);
                cell.SetBorder(border);
                cell.SetPaddingBottom(0f);
                cell.SetPaddingTop(0f);
                table.AddCell(cell);
            }
            blockElements.Add(table); return table;
        }
        public Paragraph CreateNewText(string text, bool addToDoc, bool IsBold, int size)
        {
            Paragraph para = new Paragraph(text);
            para.SetFont(currentFont);
            para.SetFontSize(size);
            if (IsBold) para.SetBold();
            if (addToDoc) blockElements.Add(para);
            return para;
        }
        public Paragraph CreateNewText(string text, int size, TextAlignment textAlignment, int spacingBetweenLine, bool addToDoc, bool IsBold)
        {
            Paragraph para = new Paragraph(text);
            para.SetFont(currentFont);
            para.SetFontSize(size);
            para.SetTextAlignment(textAlignment);
            para.SetFixedLeading(spacingBetweenLine);               // Create spacing of line when you leave
            if (IsBold) para.SetBold();
            if (addToDoc) blockElements.Add(para);
            return para;
        }
        private byte[] CreateNewImageFromUrl(string url)
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
        public Image CreatePdfImage(string url)
        {
            var imageByte = CreateNewImageFromUrl(url);
            ImageData data = ImageDataFactory.Create(imageByte);
            Image image = new Image(data);
            return image;
        }
        public Table CreateNewTableCollection(List<object> collection)
        {
            int collectionCount = collection.Count;
            List<float> widthValues = new List<float>();
            List<PropertyInfo> sortedProperties = new List<PropertyInfo> { };
            Type elementType = collection.First().GetType();
            var propertyInfos = elementType.GetProperties();
            int propertyInfosCount = propertyInfos.Count();
            float[] widthArray = new float[propertyInfosCount];

            // CREATE NEW HEADER OF TABLE
            if (collectionCount == IText7Param.ZERO || collection == null) return default;
            if (headerFomats == null)
            {
                int index = 0;
                headerFomats = new List<HeaderConfig>();
                foreach (var property in propertyInfos)
                {
                    headerFomats.Add(new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName = property.Name,
                        DisplayColumnName = property.Name,
                        ordinalNumber = index + 1,
                        Width = IText7Param.WIDTH_COLUMN_TABLE_DEFAULT,
                    });
                    widthArray[index] = IText7Param.WIDTH_COLUMN_TABLE_DEFAULT;
                    index++;
                }
            }
            else
            {
                headerFomats = headerFomats.OrderBy(x => x.ordinalNumber).ToList();   // update lại thứ tự của column
                int index = 0;
                foreach (var item in headerFomats)
                {
                    widthArray[index] = item.Width;
                    index++;
                }
            }


            Table collectionTable = new Table(UnitValue.CreatePercentArray(widthArray));
            collectionTable.SetPadding(IText7Param.PADING_DEFAULT);
            collectionTable.SetWidth(UnitValue.CreatePercentValue(IText7Param.PERCENT_OF_TABLE_DEFAULT));
            foreach (var content in headerFomats)
            {
                var value = propertyInfos.FirstOrDefault(x => x.Name.Trim() == content.ColumnName);
                sortedProperties.Add(value);
                var text = CreateNewText(content.DisplayColumnName, false, false, 11);

                text.SetPadding(0f);
                text.SetTextAlignment(content.TextAlignment);
                Cell cell = new Cell();

                cell.SetBackgroundColor(ITEXT7_COLORS.BINH_ANH_BLUE);
                cell.Add(text);
                collectionTable.AddCell(cell);
            }
            // CREATE NEW CONTENT OF TABLE
            foreach (var row in collection)
            {
                foreach (var column in sortedProperties)
                {
                    var name = column.Name;
                    var value = column.GetValue(row, null);
                    var text = CreateNewText(value.ToString(), false, false, 10);
                    text.SetPadding(0f);
                    Cell cell = new Cell();
                    cell.Add(text);
                    cell.SetKeepTogether(true);
                    collectionTable.AddCell(cell);
                }
            }
            blockElements.Add(collectionTable);
            return collectionTable;
        }
        public void CreateNewTableCollection()
        {
            throw new NotImplementedException();
        }
        public void CreateHeaderConfig(List<HeaderConfig> headerConfigs)
        {
            headerFomats = headerConfigs;
        }
        public void CreateNewFootter()
        {
            List<string> headerContent = new List<string>
            {
                "CHỮ KÍ CỦA HỌC VIÊN \n (Kí rõ họ tên)",
                "CÔNG TY TNHH VIẾT TIẾN HÙNG \n GIÁM ĐỐC \n\n\n\n Nguyễn Viết Minh Tiến",
            };
            CreateNewTable(headerContent, TextAlignment.CENTER, iText.Layout.Borders.Border.NO_BORDER);
        }
        public void CloseDocument()
        {
            foreach (var item in blockElements) document.Add(item);
            document.Close();
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
}
