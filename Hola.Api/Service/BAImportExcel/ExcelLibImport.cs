using Hola.Api.Service.ExcelServices.TestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hola.Api.Service.BAImportExcel
{
    public class ExcelLibImport
    {
        private int wordSheetDefault = 0;
        public List<T> ConvertToList<T>(ExcelImportRequest request)
        {
            try
            {
                if (request.WorkSheet > 0)
                {
                    request.WorkSheet -= 1;
                }
                else
                {
                    request.WorkSheet = wordSheetDefault;
                }

                using (var stream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    request.File.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        // Connect to work space
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets[request.WorkSheet];
                        var rowCount = worksheet.Dimension.End.Row;
                        var colCount = worksheet.Dimension.End.Column;
                        int endRow = rowCount - request.PaddingBottom;
                        return LoadDatabase<T>(worksheet, request.StartRow, endRow, request.HeaderNames);
                    }
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        public List<T> LoadDatabase<T>(ExcelWorksheet worksheet, int startRow, int endRow, List<string> headerNames)
        {
            Type objectType = typeof(T);
            var properties = objectType.GetProperties().ToList();
            List<T> listAnyThing = new List<T>();
            for (int row = startRow; row <= endRow; row++)
            {
                List<MapDataConfig> values = new List<MapDataConfig>();
                for (int i = 0; i < headerNames.Count; i++)
                {
                    try
                    {
                        object? value = worksheet.Cells[row, i + 1].Value;
                        if (value == null)
                        {
                            return listAnyThing;
                        }
                        values.Add(new MapDataConfig
                        {
                            ColumnName = headerNames[i],
                            Value = (value != null) ? value.ToString() : string.Empty
                        });
                    }
                    catch
                    {
                        continue;
                    }
                }
                var item = CreateNewIntanceByProperties<T>(values, properties);
                listAnyThing.Add(item);
            }
            return listAnyThing;
        }
        public T CreateNewIntanceByProperties<T>(List<MapDataConfig> data, List<PropertyInfo> properties)
        {
            try
            {
                var obj = (T)Activator.CreateInstance(typeof(T));
                int i = 0;
                foreach (var item in properties)
                {
                    string columnName = item.Name;
                    var cellValue = data.Where(x => x.ColumnName == columnName).FirstOrDefault();
                    if (cellValue != null)
                    {
                        item.SetValue(obj, Convert.ChangeType(cellValue.Value, item.PropertyType), null);
                        i = i + 1;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
    }
}
