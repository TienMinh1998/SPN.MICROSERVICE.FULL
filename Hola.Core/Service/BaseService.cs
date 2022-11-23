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
            _options = options;
            if (State ==true  && string.IsNullOrEmpty(rawConnection))
            {
                rawConnection = _options.Value.Connection + "Database=";
            }
            else 
            {
                if (State==false)
                {
                    GetConnection(_options.Value.Connections);
                }
            }
        }

        private async Task GetConnection(List<string> connections)
        {
            foreach (var con in connections)
            {
                using (var l_oConnection = new NpgsqlConnection(con + "Database=commond_db"))
                {
                    try
                    {
                        l_oConnection.Open();
                        rawConnection = con + "Database=";
                        break;
                    }
                    catch (NpgsqlException)
                    {
                        continue;
                    }
                }
            }
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
                State = true;
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
                State = false;
                throw;
            }
        }
    }

}
