using Hola.Core.Enums;

namespace Hola.Core.Model.CommonModel
{
    public class GetUserActiveTokenResponseCore
    {
        public long TokenId { get; set; }

        public long? DeviceId { get; set; }

        public long UserId { get; set; }

        public LoginProvider LoginProvider { get; set; }
    }
}