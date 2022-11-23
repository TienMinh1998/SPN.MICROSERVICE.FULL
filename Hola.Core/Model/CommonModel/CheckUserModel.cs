using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Model.CommonModel
{
    public class CheckUserModel
    {
        public string UserId { get; set; }
        public bool PhoneNumberExist { get; set; }
        public string PhoneNumber { get; set; }
        public short RoleId { get; set; }
        public string Username { get; set; }
    }
}
