using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using OfficeOpenXml.Drawing;

namespace Hola.Api.Service.ExcelServices
{
    public class ExcelService
    {
        private string fontfamily = "Times New Roman";
        private string _title;       
        private int _totalColumn;    
        private string _columnEnd;    
        private int _startContentIndex; 
        private int _rowEndIndex;
        private List<string> _heardername;
        private List<int> _columnWidths;
        private List<string> listColumn = new List<string> {"A","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U",
            "V","W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR",
            "AS","AT","AU","AV","AW","AX","AY"
            };
        private ExcelWorksheet ws;
        private readonly ExcelSetting _setting;
        public ExcelService(ExcelSetting setting)
        {
             _setting = setting;
            _heardername = _setting.Hearders;
            _columnWidths = _setting.WidthHearders;
        }
        public async Task ExportFile<T>(List<T> items)
        {
            // Setting Environment
                FileInfo file = new FileInfo(_setting.URlFile.Trim());
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                if (file.Exists) file.Delete();
                using var package = new ExcelPackage(file);
                ws = package.Workbook.Worksheets.Add(_setting.SheetName);
                Type modelType = typeof(T);
                _totalColumn = modelType.GetProperties().Count();
                _title = _setting.Title;
            // Setting title infomation
                InsertContent(1, 1, 3, "SỞ GIAO THÔNG VẬN TẢI THÁI BÌNH");
                InsertContent(2, 1, 3, "Trung tâm Bình Anh", true, ExcelHorizontalAlignment.Center);
                InsertContent(3, 1, 3, "------------", true, ExcelHorizontalAlignment.Center);
                InsertContent(1, 4, 4, "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM",true, ExcelHorizontalAlignment.Center);
                InsertContent(2, 4, 4, "Độc lập - Tự do - Hạnh Phúc", true, ExcelHorizontalAlignment.Center);
                InsertContent(3, 4, 4, "------------", true, ExcelHorizontalAlignment.Center);
                InsertContentItalic(5, 4, 4, "Hà nội, ngày 27 tháng 1 năm 2023", true, ExcelHorizontalAlignment.Right);
                SettingTitle(ws, "BÁO CÁO CHI TIẾT CỦA KHÓA HỌC THỰC HÀNH LÁI XE",6);
                SettingTitle(ws, "II. THÔNG TIN QUÁ TRÌNH ĐÀO TẠO", _setting.StartRow - 1);
                SettingContent(items, ws, _setting.StartRow);
                SetColorForColumn(ws, "Từ Vựng", "#C6E0B4");
                SettingFootter(1, "Tổng số từ của topic:" + items.Count().ToString(), "#EDEDED");
                SettingFootter(2, "Trạng thái hoàn thành", "#EDEDED");
                SettingFootter(5, "XÁC NHẬN CỦA HỌC VIÊN", 3);
                SettingFootter(6, "(kí rõ họ tên)", 3);
                SettingFootter(5, "CƠ SỞ ĐÀO TẠO", -3);
                SettingFootter(6, "(kí tên, đóng dấu)", -3);
            // Setting student infomation
                InsertContent(8, 1, 4, "I. THÔNG TIN HỌC VIÊN");
                InsertContent(9, 1, 4, " 1. Họ và tên : NGUYỄN NGỌC DIỆP");
                InsertContent(10, 1, 4, " 2. Mã học viên : 1475H220426100");
                InsertContent(11, 1, 4, " 3. Ngày sinh: 01/01/1990");
                InsertContent(12, 1, 4, " 4. Mã khóa học : 75024K100B2");
                InsertContent(13, 1, 4, " 5. Hạng đào tạo : B2");
                InsertContent(14, 1, 4, " 6. Cơ sở đào tạo : Trung tâm Bình Anh");
                
                int index = 1;
                foreach (var item in _columnWidths)
                {
                    ws.Column(index).Width = item;
                    index++;
                }
            // Import Excel 
                if (!string.IsNullOrEmpty(_setting.ExcelImage.ImageUrl))
                {
                    FileInfo fileInfo = new FileInfo(_setting.ExcelImage.ImageUrl);
                    ExcelPicture pic = ws.Drawings.AddPicture("BA_picture", fileInfo);
                    pic.SetPosition(_setting.ExcelImage.RowIndex,0,
                                    _setting.ExcelImage.ColumnIndex,0);
                    pic.SetSize(_setting.ExcelImage.Width,
                                _setting.ExcelImage.Height);
                }
                await package.SaveAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws">Context</param>
        /// <param name="margin">Khoảng cách đến dòng có nội dung cuối cùng</param>
        /// <param name="content">Nội dung muốn chèn vào footer</param>
        private void SettingFootter( int margin, string content,string _color)
        {
            var rowIndex = _rowEndIndex + margin;
            ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(rowIndex).Style.Font.Bold = true;
            ws.Row(rowIndex).Style.Font.Size = 10;
            Color colFromHex = ColorTranslator.FromHtml(_color);
            var header = GetMergeString(rowIndex);
            ws.Cells[header].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[header].Style.Fill.BackgroundColor.SetColor(colFromHex);

            ws.Cells[$"A{rowIndex}"].Value = content;
            var cellAddress = GetMergeString(rowIndex);
            ws.Cells[cellAddress].Merge = true;
            Setborder(cellAddress);

        }
        private void SettingFootter(int margin, string content, int step)
        {
            var rowIndex = _rowEndIndex + margin;
            ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(rowIndex).Style.Font.Bold = true;
            // Nếu Step > 0 là gán xuôi
            // Nếu step < 0 là gán ngược
            if (step>0)
            {
                ws.Cells[$"A{rowIndex}"].Value = content;
                var cellAddress = MergeColumn(rowIndex, step);
                ws.Cells[cellAddress].Merge = true;
                ws.Cells[cellAddress].Style.Font.Name = fontfamily;
            }
            else
            {
                // get KEy 
                var endcolumn = _columnEnd;
                var indexStart = listColumn.FindIndex(x => x.Equals(endcolumn))+step;
                var key = listColumn[indexStart];
                ws.Cells[$"{key}{rowIndex}"].Value = content;
                var cellAddress = MergeColumn(key, rowIndex, Math.Abs(step));
                ws.Cells[cellAddress].Merge = true;
                ws.Cells[cellAddress].Style.Font.Name = fontfamily;
            }
        }
        private void InsertContent(int rowIndex,int startColumnIndex,int step, string content)
        {
            ws.Row(rowIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Row(rowIndex).Style.Font.Bold = true;
            ws.Row(rowIndex).Style.Font.Size = 10;
            var key = listColumn[startColumnIndex];

            ws.Cells[$"{key}{rowIndex}"].Value = content;
            var cellAddress = MergeColumn(rowIndex,startColumnIndex,step);
            ws.Cells[cellAddress].Merge = true;
            ws.Cells[cellAddress].Style.Font.Name = fontfamily;
        }
        private void InsertContent(int rowIndex, int startColumnIndex, int step, string content,bool isBold, ExcelHorizontalAlignment alignment)
        {
            ws.Row(rowIndex).Style.HorizontalAlignment = alignment;
            ws.Row(rowIndex).Style.Font.Bold = isBold;
            ws.Row(rowIndex).Style.Font.Size = 10;
            var key = listColumn[startColumnIndex];

            ws.Cells[$"{key}{rowIndex}"].Value = content;
            var cellAddress = MergeColumn(rowIndex, startColumnIndex, step);
            ws.Cells[cellAddress].Merge = true;
            ws.Cells[cellAddress].Style.Font.Name = fontfamily;
        }
        private void InsertContentItalic(int rowIndex, int startColumnIndex, int step, string content, bool isBold, ExcelHorizontalAlignment alignment)
        {
            ws.Row(rowIndex).Style.HorizontalAlignment = alignment;
            ws.Row(rowIndex).Style.Font.Bold = isBold;
            ws.Row(rowIndex).Style.Font.Italic = true;

            ws.Row(rowIndex).Style.Font.Size = 10;
            var key = listColumn[startColumnIndex];

            ws.Cells[$"{key}{rowIndex}"].Value = content;
            var cellAddress = MergeColumn(rowIndex, startColumnIndex, step);
            ws.Cells[cellAddress].Merge = true;
            ws.Cells[cellAddress].Style.Font.Name = fontfamily;
        }
        private void SetColorForColumn(ExcelWorksheet ws,string columnName,string color)
        {
            var index = _heardername.FindIndex(x=>x.Equals(columnName,StringComparison.OrdinalIgnoreCase))+1;
            var col = listColumn[index];
            var area = $"{col}{_startContentIndex+1}:{col}{_rowEndIndex}";

            Color colFromHex = System.Drawing.ColorTranslator.FromHtml(color);
            ws.Cells[area].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[area].Style.Fill.BackgroundColor.SetColor(colFromHex);
        }
        private void SettingContent<T>(List<T> people,ExcelWorksheet ws, int startRow)
        {

            // Config header
            ws.Row(startRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(startRow).Style.Font.Bold = true;
            string configStartRow = $"A{startRow}";
            _startContentIndex = startRow;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#548235");
            var header = GetMergeString(startRow);
            ws.Cells[header].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[header].Style.Fill.BackgroundColor.SetColor(colFromHex);

             _rowEndIndex = startRow + people.Count;
            string rangeBorder = $"A{startRow-1}:{_columnEnd}{_rowEndIndex}";
          
            var modelTable = ws.Cells[rangeBorder];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            var range = ws.Cells[configStartRow].LoadFromCollection(people, true);
            range.AutoFitColumns();
            // configName
            for (int i = 1; i <= _totalColumn; i++)
                ws.Cells[$"{listColumn[i]}{startRow}"].Value =_heardername[i-1];
        }
        private string GetMergeString(int rowIndex)
        {
            var endIndex = listColumn[_totalColumn];
            _columnEnd = endIndex;
            string result = $"A{rowIndex}:{endIndex}{rowIndex}";
            return result;
        }
        private string MergeColumn(int rowIndex, int step)
        {
            var endIndex = listColumn[step];
            string result = $"A{rowIndex}:{endIndex}{rowIndex}";
            return result;
        }
        private string MergeColumn(int rowIndex,int columnIndex, int step)
        {
            var endIndex = listColumn[columnIndex+step-1];
            var keyStart = listColumn[columnIndex];
            string result = $"{keyStart}{rowIndex}:{endIndex}{rowIndex}";
            return result;
        }
        private string MergeColumn(string key, int rowIndex, int step)
        {
            var endIndex = _columnEnd;
            string result = $"{key}{rowIndex}:{endIndex}{rowIndex}";
            return result;
        }
        public class ExcelSetting
        {
            public string Title { get; set; }
            public int CountRowMerge { get; set; } = 0;
            public string SheetName { get; set; }
            public int StartRow { get; set; } = 3;
            public string URlFile { get; set; }
            public ImageProperties ExcelImage { get; set; } = new ImageProperties() { 
            ColumnIndex =0,
            RowIndex = 0,
            Height = 150,
            Width =  120
            };
            public List<string> Hearders { get; set; }
            public List<int> WidthHearders { get; set; }

        } 
        public void SettingTitle(ExcelWorksheet ws,string title, int rowIndex)
        {
            // Formats the header
            ws.Cells[$"A{rowIndex}"].Value = title;
            var m = GetMergeString(rowIndex);
            ws.Cells[m].Merge = true;

            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(rowIndex).Style.Font.Size = 16;
            ws.Row(rowIndex).Style.Font.Bold = true;
            ws.Row(rowIndex).Style.Font.Color.SetColor(Color.Black);
        }
        public void Setborder(string address)
        {
            var modelTable = ws.Cells[address];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
    }
}
