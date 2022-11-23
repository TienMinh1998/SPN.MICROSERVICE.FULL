using System.Data;
using System.Data.Common;
using Hola.Core.Model;

namespace Hola.Core.Provider
{
    public static class Providers
    {
        private static DbConnection connection;

        public static DbConnection GetConnection(SettingModel setting)
        {
            if (setting.Provider == "Sql")
            {

                connection = SqlProvider.GetSqlConnection(setting.Connection);
                return connection;
            }
            else
            {

                connection = PostgreProvider.GetPostgreConnection(setting.Connection);
                return connection;
            }
        }


        public static void Close()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Dispose();
                connection.Close();
            }
        }
    }
}