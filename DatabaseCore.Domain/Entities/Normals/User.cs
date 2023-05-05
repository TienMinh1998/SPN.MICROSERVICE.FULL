using DatabaseCore.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class User : BaseEntity<int>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string DeviceToken { get; set; }
        public string Name { get; set; }
        public int isnotification { get; set; }
        public string Avartar { get; set; }
        public int Status { get; set; }             // kiểm tra xem đã được kích hoạt chưa
    }
}
