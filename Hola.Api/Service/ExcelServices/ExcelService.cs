using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
namespace Hola.Api.Service.ExcelServices
{
    public class ExcelService
    {
        private string fontfamily = "Times New Roman";
        private string _title;        // Tiêu đề
        private int _totalColumn;     // Tổng số cột
       private string _columnEnd;     // Cột cuối dùng
        private int _startContentIndex; 
        private int _rowEndIndex;
        private List<string> heardername = new List<string>()
        {
            "Từ vựng",
            "Phiên âm",
            "Nghĩa Tiếng Anh",
            "Nghĩa tiếng việt",
            "Ghi chú",
            "ID",
            "Trạng thái"
        };
        private  List<string> listColumn = new List<string> {"A","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U",
            "V","W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR",
            "AS","AT","AU","AV","AW","AX","AY"
            };
        private ExcelWorksheet ws;

        public async Task ExportFile<T>(List<T> people, ExcelSetting settingOptions)
        {
            FileInfo file = new FileInfo(settingOptions.URlFile.Trim());
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file.Exists) file.Delete();
            using var package = new ExcelPackage(file);
            ws = package.Workbook.Worksheets.Add(settingOptions.SheetName);
            Type modelType = typeof(T);
             _totalColumn = modelType.GetProperties().Count();
             _title = settingOptions.Title;

            // Set Start Rows content
           

            SettingTitle(ws,  " III. BÁO CÁO TỪ VỰNG CỦA CHỦ ĐIỂM TOPIC ĐÃ CHỌN",settingOptions.StartRow-1);
            SettingContent(people,ws,settingOptions.StartRow);
            SetColorForColumn(ws,"Từ Vựng", "#C6E0B4");
            SettingFootter(1,"Tổng số từ của topic:"+ people.Count().ToString(),"#EDEDED");
            SettingFootter(2, "Trạng thái hoàn thành", "#EDEDED");
            SettingFootter(4, "XÁC NHẬN CỦA HỌC VIÊN",3);
            SettingFootter(5, "(kí rõ họ tên)", 3);
            SettingFootter(4, "CƠ SỞ ĐÀO TẠO", -3);
            SettingFootter(5, "(kí tên, đóng dấu)", -3);
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
        private void SetColorForColumn(ExcelWorksheet ws,string columnName,string color)
        {
            var index = heardername.FindIndex(x=>x.Equals(columnName,StringComparison.OrdinalIgnoreCase))+1;
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
            for (int i = 1; i < _totalColumn; i++)
                ws.Cells[$"{listColumn[i]}{startRow}"].Value =heardername[i-1];
        }
        private void SettingHeader(ExcelWorksheet ws, string title, int totalColumn)
        {
           

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
        }
        public void SettingTitle(ExcelWorksheet ws,string title, int rowIndex)
        {
            // Formats the header
            ws.Cells[$"A{rowIndex}"].Value = title;
            var m = GetMergeString(rowIndex);
            ws.Cells[m].Merge = true;

            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Row(rowIndex).Style.Font.Size = 16;
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
