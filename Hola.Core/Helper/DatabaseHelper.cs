using Hola.Core.Model;
using Hola.Core.Provider;
using Hola.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Npgsql;

namespace Hola.Core.Helper
{
    public static class DatabaseHelper
    {
        public static DataTable ExcuteSql(string sql, SettingModel setting)
        {
            DataTable dt = new DataTable();
            try
            {
                var connection = Providers.GetConnection(setting);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (setting.Provider=="Sql")
                {
                    var ada = new SqlDataAdapter(command as SqlCommand);
                    ada.Fill(dt);
                }
                else
                {
                    var ada = new NpgsqlDataAdapter(command as NpgsqlCommand);
                    ada.Fill(dt);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                Providers.Close();
            }
            return dt;
        }
        /// <summary>
        /// Returns the number of rows changed when you excecute SQLText
        /// </summary>
        /// <param name="sql">SQL Text</param>
        /// <param name="setting"></param>
        /// <returns>Return int</returns>
        public static int ExcuteNonQuery(string sql, SettingModel setting)
        {
            try
            {
                var connection = Providers.GetConnection(setting);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                return command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 0;
            }
            finally
            {
                Providers.Close();
            }
        }
        /// <summary>
        ///  Executes the query, and returns the first column of the first row in the resultset
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="setting"></param>
        /// <returns>Return a Object </returns>
        public static object? ExecuteScalar(string sql, SettingModel setting)
        {
            try
            {
                var connection = Providers.GetConnection(setting);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                return command.ExecuteScalar();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 0;
            }
            finally
            {
                Providers.Close();
            }
        }
        /// <summary>
        /// Return value that Type is Int
        /// </summary>
        /// <param name="sql">Sqltext</param>
        /// <param name="setting">Ceting Connection</param>
        /// <returns></returns>
        public static async Task<DataTable> ExcuteSqlAsync(string sql, SettingModel setting)
        {
            DataTable dt = new DataTable();
            try
            {
                var connection = Providers.GetConnection(setting);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                dt.Load(await command.ExecuteReaderAsync());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                Providers.Close();
            }

            return dt;
        }

        public static async Task<int> ExcuteNonQueryAsync(string sql, SettingModel setting)
        {
            try
            {
                var connection = Providers.GetConnection(setting);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 0;
            }
            finally
            {
                Providers.Close();
            }
        }

        #region Execute and Convert

        public static List<T> ExcuteQueryToList<T>(string sql, SettingModel setting)
        {
            DataTable dt = ExcuteSql(sql, setting);
            return Converter.DataTableToList<T>(dt);
        }

        public static List<Dictionary<string, object>> ExcuteQueryToDict(string sql, SettingModel setting)
        {
            DataTable dataTable = ExcuteSql(sql, setting);
            return Converter.ParseTableToDictationary(dataTable);
        }

        public static async Task<List<T>> ExcuteQueryToListAsync<T>(string sql, SettingModel setting)
        {
            DataTable dt = await ExcuteSqlAsync(sql, setting);
            return Converter.DataTableToList<T>(dt);
        }

        public static async Task<List<Dictionary<string, object>>> ExcuteQueryToDictAsync(string sql, SettingModel setting)
        {
            DataTable dataTable = await ExcuteSqlAsync(sql, setting);
            return Converter.ParseTableToDictationary(dataTable);
        }

        public static object ExecuteFunction(string sql, SettingModel setting)
        {
            try
            {
                var connection = Providers.GetConnection(setting);
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                return command.ExecuteScalar();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 0;
            }
            finally
            {
                Providers.Close();
            }
        }
        #endregion Execute and Convert

        
    }
}