using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Helper
{
    public class LogsToServerConsole
    {
        public static void Log(string message)
        {
            Console.WriteLine($"Time {DateTime.UtcNow} - Message :  {message}" );
        }
    }
}
