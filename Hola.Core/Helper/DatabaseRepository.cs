using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Hola.Core.Helper
{
    public class DatabaseRepository<T> where T : class
    {
        public string ConnectionString;
        public string Schemas;


        public DatabaseRepository(string connectionString, string schemas)
        {
            ConnectionString = connectionString;
            Schemas = schemas;
        }
        public void ExecuteAsync(string sql)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                connection.ExecuteAsync(sql);
                connection.Close();
            };
        }

    }
}
