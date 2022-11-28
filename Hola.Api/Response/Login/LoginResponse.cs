using DatabaseCore.Domain.Entities.Normals;

namespace Hola.Api.Response.Login
{
    public class LoginResponse
    {
        public User user { get; set; }
        public string Token { get; set; }
    }
}
