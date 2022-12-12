using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCommon.EntitiesModel
{
    public class UserManualModel
    {
        public int Fk_Grannar_Id { get; set; }
        public string Used { get; set; }          // Cách sử dụng
        public string Example { get; set; }        // Example :  Ví dụ cho cách sử dụng
        public string DetailExample { get; set; }  // DetailExample Giải thích chi tiết cách sử dụng Example
    }
}
