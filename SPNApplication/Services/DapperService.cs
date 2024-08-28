using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPNApplication.Services
{
    public class DapperService : IDapperService
    {
        private string _connectionString;

        public DapperService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HolaCoreConnectionString");
        }

        public T QueryFirst<T>(string query, object parameters = null)
        {
            try
            {
                using (NpgsqlConnection conn
                       = new NpgsqlConnection(_connectionString))
                {
                    return conn.QueryFirst<T>(query, parameters);
                }
            }
            catch (Exception)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string query, object parameters = null)
        {
            try
            {
                using (NpgsqlConnection conn
                       = new NpgsqlConnection(_connectionString))
                {
                    var response = await conn.QueryAsync<T>(query, parameters);
                    return response;
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public T QueryFirstOrDefault<T>(string query, object parameters = null)
        {
            try
            {
                using (NpgsqlConnection conn
                       = new NpgsqlConnection(_connectionString))
                {
                    return conn.QueryFirstOrDefault<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public T QuerySingle<T>(string query, object parameters = null)
        {
            try
            {
                using (NpgsqlConnection conn
                       = new NpgsqlConnection(_connectionString))
                {
                    return conn.QuerySingle<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public T QuerySingleOrDefault<T>(string query, object parameters = null)
        {
            try
            {
                using (NpgsqlConnection conn
                       = new NpgsqlConnection(_connectionString))
                {
                    return conn.QuerySingleOrDefault<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
                return default; //Or however you want to handle the return
            }
        }

        public async Task Execute(string query, object parameters = null)
        {
            try
            {
                using (NpgsqlConnection conn
                       = new NpgsqlConnection(_connectionString))
                {
                    await conn.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //Handle the exception
            }
        }
    }
}
