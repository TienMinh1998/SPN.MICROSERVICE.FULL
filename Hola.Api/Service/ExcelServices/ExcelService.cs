using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using System.Net.Http;
using Hola.Api.Service.ExcelServices.Enum;
using Hola.Api.Service.ExcelServices.DataConfig;

namespace Hola.Api.Service.ExcelServices
{
    public class ExcelService
    {
        private int _totalColumn;
        private string _columnEnd;
        private int _rowEndIndex;
        private int lastRowWithData;

        private List<string> listColumn = new List<string> {"A","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U",
            "V","W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR",
            "AS","AT","AU","AV","AW","AX","AY"
            };

        private ExcelWorksheet ws;
        private readonly ReportConfiguration _setting;

        public ExcelService(ReportConfiguration reportConfiguration)
        {
            _setting = reportConfiguration;
        }
        public async Task<string> ExportExcel<T, T1, T2>()
        {
            try
            {
                // Setting Environment
                string filename = DateTime.Now.ToString("ssddMMyyyy") + "blank.xlsx";
                string fileURL = Path.Combine(Directory.GetCurrentDirectory(), $"ExcelTemplate/{filename}").Trim();
                FileInfo file = new FileInfo(fileURL);
                if (file.Exists) file.Delete();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var package = new ExcelPackage(file);
                ws = package.Workbook.Worksheets.Add("Report");

                #region Config Header of Report
                foreach (var item in _setting.ExHeader)
                {
                    if (item.Visible)
                    {
                        InsertContent(item.StartRow, item.StartColumn, item.ColumnSpan, item.TopContent, item.FontWeigh, item.Align, item.Size, item.FontFamily);
                        InsertContent(item.StartRow + 1, item.StartColumn, item.ColumnSpan, item.BottomContent, "B", item.Align, item.Size, item.FontFamily);
                        InsertContent(item.StartRow + 2, item.StartColumn, item.ColumnSpan, "------------", "B", item.Align, item.Size, item.FontFamily);
                    }
                }
                UpdateLastRow();
                #endregion

                #region Config Body Content of Report
                int paramindex = 0;
                foreach (var item in _setting.Body.Datagriviews)
                {
                    lastRowWithData += item.MarginTop;
                    if (item.Type == DATAGRIVIEW_TYPE.TABLE)
                    {
                        switch (paramindex)
                        {
                            case 0:
                                var data = (List<T>)item.Data;
                                SettingContent(data, lastRowWithData + 3, item.Headers);
                                break;
                            case 1:
                                var dataT1 = (List<T1>)item.Data;
                                SettingContent(dataT1, lastRowWithData + 3, item.Headers);
                                break;
                            case 2:
                                var dataT2 = (List<T2>)item.Data;
                                SettingContent(dataT2, lastRowWithData + 3, item.Headers);
                                break;
                            default:
                                break;
                        }
                        InsertContent(lastRowWithData + 1, item.StartColumnOfContent, _totalColumn, item.Content, item.FontWeigh, item.Align, item.Size, item.FontFamily);
                        InsertContent(lastRowWithData + 2, item.StartColumnOfContent, _totalColumn, item.SubContent, item.FontWeigh, item.Align, item.SizeSub, item.FontFamily);
                        if (item.Footers != null) item.Footers.ForEach(x => SettingFootter(x));
                        UpdateLastRow();
                        paramindex++;
                    }
                    else if (item.Type == DATAGRIVIEW_TYPE.TEXT)
                    {
                        // Trường hợp là tiêu đề bình thường
                        int rowStart = 0;
                        int rowEnd = 0;
                        string charStart, ChartEnd;
                        InsertContent(lastRowWithData + 1, item.StartColumnOfContent, item.MergeColumnTitle, item.Content, item.FontWeigh, item.Align, item.Size, item.FontFamily);
                        InsertContent(lastRowWithData + 2, item.StartColumnOfContent, item.MergeColumnTitle, item.SubContent, item.FontWeigh, item.Align, item.SizeSub, item.FontFamily);
                        UpdateLastRow();
                        int startIndex = lastRowWithData + 2;
                        rowStart = startIndex;
                        for (int i = 0; i < item.InfomationText.Count; i++)
                        {
                            InsertContent(startIndex, 1, 5, item.InfomationText[i], "", "left", 10, item.FontFamily);
                            startIndex++;
                        }
                        rowEnd = startIndex;
                        charStart = "A";
                        ChartEnd = listColumn[item.MergeColumnTitle];
                        string rangeBorder = $"{charStart}{rowStart - 1}:{ChartEnd}{rowEnd}";
                        ws.Cells[rangeBorder].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Gray);

                        // Xử lý ảnh
                        if (item.Images.Count > 0)
                        {
                            item.Images.ForEach(x => ImportImage(x.ImageUrl, x.RowIndex, x.ColumnIndex, x.Width, x.Height, x.Name, x.MarginLeft, x.MarginTop));
                        }
                        UpdateLastRow();
                    }
                }
                #endregion


                #region Add Text Flus

                int textPlusCount = _setting.Texts.Count;
                if (textPlusCount > 0)
                {
                    foreach (var text in _setting.Texts)
                        InsertContent
                        (text.Row, text.Col, text.ColSpan, text.Content, text.FontWeigh, text.Align, text.Size, text.FontFamily);
                }

                #endregion


                ws.PrinterSettings.Orientation = eOrientation.Portrait;
                await package.SaveAsync();
                return fileURL;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Support Function

        private void SettingFootter(FootterConfig footerItem)
        {
            try
            {
                var rowIndex = _rowEndIndex + footerItem.MarginTop + 1;
                ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var cellAddress = MergeColumn(rowIndex, footerItem.StartColumn, footerItem.MergeRow);
                ws.Cells[cellAddress].Value = footerItem.Content;
                ws.Cells[cellAddress].Merge = true;
                Setborder(cellAddress);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private void InsertContent(int rowIndex, int startColumnIndex, int step, string content, string fontWeigh, string alignment, int size, string fontFamily)
        {

            ws.Row(rowIndex).Style.Font.Size = size;
            var key = listColumn[startColumnIndex];

            ws.Cells[$"{key}{rowIndex}"].Value = content;
            var cellAddress = MergeColumn(rowIndex, startColumnIndex, step);

            ws.Cells[cellAddress].Style.Font.Name = fontFamily;
            ws.Cells[cellAddress].Style.WrapText = true;

            if (alignment.Trim().ToLower() == "center")
            {
                ws.Cells[cellAddress].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else
            {
                if (alignment.Trim().ToLower() == "left")
                {
                    ws.Cells[cellAddress].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                else
                {
                    ws.Cells[cellAddress].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
            }

            string fw = fontWeigh.Trim().ToUpper();
            ws.Cells[cellAddress].Style.Font.Bold = false;
            if (fw.Contains("B"))
                ws.Cells[cellAddress].Style.Font.Bold = true;
            if (fw.Contains("I"))
                ws.Cells[cellAddress].Style.Font.Italic = true;
            if (fw.Contains("U"))
                ws.Cells[cellAddress].Style.Font.UnderLine = true;
        }
        private void SettingContent<T>(List<T> colection, int startRow, List<HeaderOfTable> listConfigColumn)
        {
            try
            {
                Type modelType = typeof(T);
                var Columns = listConfigColumn.OrderBy(x => x.Location).ToList();
                var listData = new List<Dictionary<string, object>> { };
                var properties = typeof(T).GetProperties();
                List<PropertyInfo> propertiesSort = new List<PropertyInfo> { };
                int rowCount = colection.Count;
                int rowIndexStart = startRow;
                int headerCount = Columns.Count;

                // get header configuration 
                int index = 1;
                PropertyInfo tmp = null;
                foreach (var item in Columns)
                {
                    var column = properties.FirstOrDefault(x => x.Name.ToLower() == item.ColumnName.ToLower());
                    if (column != null)
                    {
                        tmp = column;
                        propertiesSort.Add(column);
                    }
                    else
                    {
                        propertiesSort.Add(tmp);
                    }
                    ws.Column(index).Width = item.Width;
                    index = index + 1;
                }

                _rowEndIndex = startRow + colection.Count;
                #region Xử lý binding cột dữ liệu

                foreach (var item in colection)
                {

                    foreach (var column in Columns)
                    {
                        if (column.ColumnSpan != 0)
                        {
                            MergeColumn(rowIndexStart, column.Location, column.ColumnSpan);
                        }
                    }
                    // SetValue
                    rowIndexStart = rowIndexStart + 1;
                    var dicCols = new Dictionary<string, object>();
                    var count = 0;
                    foreach (var property in propertiesSort)
                    {
                        count++;
                        if (property != null)
                        {
                            var name = property.Name;
                            var value = property.GetValue(item, null);
                            if (dicCols.ContainsKey(name)) { name = name + "_" + count; }
                            dicCols.Add(name, value);
                        }
                    }
                    listData.Add(dicCols);
                }

                // Merge column, SetAlign
                foreach (var column in Columns)
                {
                    // Align
                    if (string.IsNullOrEmpty(column.TextAlign))
                    {
                        int startrow = startRow;
                        int endRow = _rowEndIndex;
                        string key = listColumn[column.Location];
                        string address = $"{key}{startRow}:{key}{endRow}";
                        ws.Cells[address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Column span
                    if (column.ColumnSpan != 0)
                    {
                        MergeColumn(rowIndexStart, column.Location, column.ColumnSpan);
                    }

                }
                #endregion

                #region Đổ dữ liệu đã Binding vào Excel, thiết lập border

                ws.Row(startRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(startRow).Style.Font.Bold = true;
                string configStartRow = $"A{startRow}";

                Color colFromHex = ColorTranslator.FromHtml("#548235");

                _totalColumn = Columns.Count;
                var endIndex = listColumn[_totalColumn];
                _columnEnd = endIndex;
                string headerAddress = $"A{startRow}:{endIndex}{startRow}";
                ws.Cells[headerAddress].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[headerAddress].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells[headerAddress].Style.Font.Color.SetColor(Color.White);
                string rangeBorder = $"A{startRow}:{_columnEnd}{_rowEndIndex}";

                Setborder(rangeBorder);
                var range = ws.Cells[configStartRow].LoadFromCollection(listData, true);
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string MergeColumn(int rowIndex, int columnIndex, int step)
        {
            var endIndex = listColumn[columnIndex + step - 1];
            var keyStart = listColumn[columnIndex];
            string result = $"{keyStart}{rowIndex}:{endIndex}{rowIndex}";
            ws.Cells[result].Merge = true;
            return result;
        }
        public void Setborder(string address)
        {
            var modelTable = ws.Cells[address];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
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

        public void ImportImage(string url, int row, int col, int width, int height, string name, int marginLeft, int marginTop)
        {
            byte[] imageBytes = GetImageFromUrl(url);
            var memoryStream = new MemoryStream(imageBytes);
            var picture = ws.Drawings.AddPicture(name, memoryStream);
            picture.SetSize(width, height);
            picture.SetPosition(row, marginTop, col, marginLeft);
        }

        class Item
        {
            public string Name { get; set; }
            public string ImageUrl { get; set; }
        }

        private void UpdateLastRow()
        {
            lastRowWithData = ws.Dimension.End.Row;
        }

        #endregion

    }
}
