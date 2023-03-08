using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Utils
{
    public static class ExtensionDateTime
    {
        public static DateTime ConvertStringToDateTime(this string strDateTime)
        {
            DateTime dateTime;
            DateTime.TryParseExact(strDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }
    }
}
