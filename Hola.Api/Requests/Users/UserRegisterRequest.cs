using System.Drawing;

namespace Hola.Api.Requests.Users
{
    public class UserRegisterRequest
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
