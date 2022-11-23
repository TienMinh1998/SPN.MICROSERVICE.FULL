using System.Data.Common;
using System.Data.SqlClient;

namespace Hola.Core.Provider
{
    public static class SqlProvider
    {
        public static DbConnection GetSqlConnection(string ConnectionString)
        {
            DbConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}