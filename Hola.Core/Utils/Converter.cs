using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Hola.Core.Utils
{
    public class Converter
    {
        public static List<T> DataTableToList<T>(DataTable dataTable)
        {
            var columnsName = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            var properties = typeof(T).GetProperties();
            return dataTable.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var propertyInfo in properties)
                {
                    if (columnsName.Contains(propertyInfo.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(propertyInfo.Name);
                        propertyInfo.SetValue(objT,
                            row[propertyInfo.Name] == DBNull.Value
                                ? null
                                : pI.PropertyType.IsEnum
                                ? Enum.Parse(pI.PropertyType, row[propertyInfo.Name].ToString())
                                : Convert.ChangeType(row[propertyInfo.Name], pI.PropertyType));
                    }
                }

                return objT;
            }).ToList();
        }
        public static List<Dictionary<string, object>> ParseTableToDictationary(DataTable dataTable)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dataTableRow in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    row.Add(dataColumn.ColumnName, dataTableRow[dataColumn]);
                }
                list.Add(row);
            }

            return list;
        }


    }
}