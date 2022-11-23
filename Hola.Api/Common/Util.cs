using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Common
{
    public static class Util
    {
        public static string ToPading(this string sqlcommand, int? currentPage, int? pageLimit)
        {
            string cmd = sqlcommand;
            var take = !pageLimit.HasValue || pageLimit.Value <= 0 ? "" : $"FETCH NEXT " + pageLimit.Value + " ROWS ONLY";
            cmd += " OFFSET " + ((currentPage ?? 1) - 1) * (pageLimit ?? 0) + " ROWS " + take;
            return cmd;
        }
    }
}
