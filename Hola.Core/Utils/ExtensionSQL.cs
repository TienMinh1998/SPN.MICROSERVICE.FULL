using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Utils
{
    public static class ExtensionSQL
    {
        /// <summary>
        /// Add Padding
        /// </summary>
        /// <param name="sqlcommand"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageLimit"></param>
        /// <returns></returns>
        public static string AddPadding(ref string sqlcommand, int? currentPage, int? pageLimit)
        {
            try
            {
                var take = !pageLimit.HasValue || pageLimit.Value <= 0 ? "" : $"FETCH NEXT " + pageLimit.Value + " ROWS ONLY";
                sqlcommand += " OFFSET " + ((currentPage ?? 1) - 1) * (pageLimit ?? 0) + " ROWS " + take;
                return sqlcommand;
            }
            catch (Exception ex)
            {
                return sqlcommand;
            }

        }
        /// <summary>
        /// Add OrderBy
        /// </summary>
        /// <param name="sqlcommand"></param>
        /// <param name="filed"></param>
        /// <param name="OrderDirection"></param>
        /// <returns></returns>
        public static string AddOrderBy(ref string sqlcommand, string filed, string OrderDirection)
        {
            try
            {
                if (!string.IsNullOrEmpty(filed)) sqlcommand += $" order by \"{filed}\" " + (string.IsNullOrEmpty(OrderDirection) ? string.Empty : OrderDirection);
                sqlcommand += string.Empty;
                return sqlcommand;
            }
            catch (Exception)
            {
                return sqlcommand;
            }

        }

        /// <summary>
        /// order by and Padding.
        /// </summary>
        /// <param name="sqlcommand"></param>
        /// <param name="column"></param>
        /// <param name="OrderDirection"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageLimit"></param>
        /// <returns></returns>
        public static string AddOrderByAndPadding(this string sqlcommand, string column, string OrderDirection, int? currentPage, int? pageLimit)
        {
            try
            {
                if (!string.IsNullOrEmpty(column)) sqlcommand += $" order by \"{column}\" " + (string.IsNullOrEmpty(OrderDirection) ? string.Empty : OrderDirection);
                var take = !pageLimit.HasValue || pageLimit.Value <= 0 ? "" : $"FETCH NEXT " + pageLimit.Value + " ROWS ONLY";
                sqlcommand += " OFFSET " + ((currentPage ?? 1) - 1) * (pageLimit ?? 0) + " ROWS " + take;
                return sqlcommand;
            }
            catch (Exception)
            {
                return sqlcommand;
            }

        }

        public async static Task<List<object>> QueryMultilple(string _connection, List<string> _listSQL)
        {
            try
            {
                List<object> listResponse = new List<object>();
                string querymultilple = string.Join(";", _listSQL);
                using (var connection = new NpgsqlConnection(_connection))
                {
                    using (var multil = connection.QueryMultiple(querymultilple))
                    {
                        foreach (var item in _listSQL)
                        {
                            if (item.Contains("COUNT"))
                            {
                                var response1 = await multil.ReadFirstAsync<int>();
                                listResponse.Add(response1);
                            }
                            else
                            {
                                var response = await multil.ReadAsync();
                                listResponse.Add(response);
                            }

                        }
                    };
                };
                return listResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Get number from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetNumber(this string str)
        {
            string b = string.Empty;
            int values = 0;
            for (int i = 0; i < str.Length; i++)
                if (Char.IsDigit(str[i])) b += str[i];
            if (b.Length > 0)
                values = int.Parse(b);
            return values;
        }
        /// <summary>
        /// Add Padding T.Criss
        /// </summary>
        /// <param name="sqlcommand"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageLimit"></param>
        /// <returns></returns>
        public static string AddPadding(this string sqlcommand, int? currentPage, int? pageLimit)
        {
            try
            {
                var take = !pageLimit.HasValue || pageLimit.Value <= 0 ? "" : $"FETCH NEXT " + pageLimit.Value + " ROWS ONLY";
                sqlcommand += " OFFSET " + ((currentPage ?? 1) - 1) * (pageLimit ?? 0) + " ROWS " + take;
                return sqlcommand;
            }
            catch (Exception)
            {

                return sqlcommand;
            }

        }
    }
}
