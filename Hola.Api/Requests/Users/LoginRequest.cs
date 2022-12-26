namespace Hola.Api.Requests.Users
{
    public class LoginRequest
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string DevideToken { get; set; }
    }

    public class LoginRequestAdmin
    {
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
