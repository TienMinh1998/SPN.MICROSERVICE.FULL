using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace Hola.Core.DapperExtension
{
    public class DapperHepperDatabase
    {

        public static List<T> QueryToList<T>(string connection, string querySQl)
        {
            if (string.IsNullOrEmpty(connection))
                throw new NullReferenceException(nameof(NpgsqlConnection));
            if (string.IsNullOrEmpty(querySQl))
                throw new NullReferenceException(nameof(NpgsqlTsQuery));
            using (var con = new NpgsqlConnection(connection))
            {
                return con.Query<T>(querySQl).ToList();
            }
        }

    }
}
