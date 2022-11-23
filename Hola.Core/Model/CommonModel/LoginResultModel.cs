using System;

namespace Hola.Core.Model.CommonModel
{
    public class LoginResultModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public int Expirationepoch { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public int RefreshTokenExpirationepoch { get; set; }
        public bool ExistingDevice { get; set; }
    }
}
