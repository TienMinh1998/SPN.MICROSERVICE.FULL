using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dapper;
using Hola.Core.Model;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;

namespace Hola.Core.Service
{
    public abstract class BaseService
    {
        /// <summary>
        /// rawConnection is raw ConnectionString
        /// State is State is Connection
        /// </summary>
        protected static string rawConnection;
        protected static bool State = true;

        private readonly IOptions<SettingModel> _options;
        protected BaseService(IOptions<SettingModel> options)
        {

        }



        protected List<T> QueryToList<T>(string connection, string querySQl)
        {
            try
            {
                State = true;
                if (string.IsNullOrEmpty(connection))
                    throw new NullReferenceException(nameof(NpgsqlConnection));
                if (string.IsNullOrEmpty(querySQl))
                    throw new NullReferenceException(nameof(NpgsqlTsQuery));
                using (var con = new NpgsqlConnection(connection))
                {

                    return con.Query<T>(querySQl).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                State = false;
                throw;
            }
        }
        protected async Task<List<T>> QueryToListAsync<T>(string connection, string querySQl)
        {
            if (string.IsNullOrEmpty(connection))
                throw new NullReferenceException(nameof(NpgsqlConnection));
            if (string.IsNullOrEmpty(querySQl))
                throw new NullReferenceException(nameof(NpgsqlTsQuery));
            using (var con = new NpgsqlConnection(connection))
            {
                var result = await con.QueryAsync<T>(querySQl);
                if (result is null || result.Count() == 0)
                    return null;
                return result.ToList();
            }
        }
        protected T FirstOrDefault<T>(string connection, string querySQl)
        {
            try
            {
                if (string.IsNullOrEmpty(connection))
                    throw new NullReferenceException(nameof(NpgsqlConnection));
                if (string.IsNullOrEmpty(querySQl))
                    throw new NullReferenceException(nameof(NpgsqlTsQuery));
                using (var con = new NpgsqlConnection(connection))
                {
                    var response = con.QueryFirstOrDefault<T>(querySQl);
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        protected async Task<T> FirstOrDefaultAsync<T>(string connection, string querySQl)
        {
            try
            {
                if (string.IsNullOrEmpty(connection))
                    throw new NullReferenceException(nameof(NpgsqlConnection));
                if (string.IsNullOrEmpty(querySQl))
                    throw new NullReferenceException(nameof(NpgsqlTsQuery));
                using (var con = new NpgsqlConnection(connection))
                {
                    var response = await con.QueryFirstOrDefaultAsync<T>(querySQl);
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        protected async Task<int> Excecute(string connection, string querySQl)
        {
            try
            {
                State = true;
                if (string.IsNullOrEmpty(connection))
                    throw new NullReferenceException(nameof(NpgsqlConnection));
                if (string.IsNullOrEmpty(querySQl))
                    throw new NullReferenceException(nameof(NpgsqlTsQuery));
                using (var con = new NpgsqlConnection(connection))
                {
                    var response = await con.ExecuteAsync(querySQl);
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                State = false;
                throw;
            }
        }
        protected async Task<int> ExecuteScalarAsync(string connection, string querySQl)
        {
            try
            {
                State = true;
                if (string.IsNullOrEmpty(connection))
                    throw new NullReferenceException(nameof(NpgsqlConnection));
                if (string.IsNullOrEmpty(querySQl))
                    throw new NullReferenceException(nameof(NpgsqlTsQuery));
                using (var con = new NpgsqlConnection(connection))
                {
                    var response = await con.ExecuteScalarAsync<int>(querySQl);
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                State = false;
                throw;
            }
        }
        protected async Task<int> ExcecuteScalarAsync(string connection, string querySQl)
        {
            try
            {
                State = true;
                if (string.IsNullOrEmpty(connection))
                    throw new NullReferenceException(nameof(NpgsqlConnection));
                if (string.IsNullOrEmpty(querySQl))
                    throw new NullReferenceException(nameof(NpgsqlTsQuery));
                using (var con = new NpgsqlConnection(connection))
                {
                    var response = await con.ExecuteScalarAsync<int>(querySQl);
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                State = false;
                throw;
            }
        }
    }

}
