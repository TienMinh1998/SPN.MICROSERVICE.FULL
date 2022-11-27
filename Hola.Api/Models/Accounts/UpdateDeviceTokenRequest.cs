namespace Hola.Api.Models.Accounts
{
    public class UpdateDeviceTokenRequest
    {
        public int UserId { get; set; }
        public string DeviceToken { get; set; }
    }
}
