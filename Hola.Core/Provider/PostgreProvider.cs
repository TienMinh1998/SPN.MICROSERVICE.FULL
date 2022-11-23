using System.Data.Common;
using Npgsql;

namespace Hola.Core.Provider
{
    public static class PostgreProvider
    {
        public static DbConnection GetPostgreConnection(string ConnectionString)
        {
            DbConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}