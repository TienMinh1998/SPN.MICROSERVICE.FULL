using System;

namespace Hola.Core.Model.CommonModel
{
    public class ListUserModel
    {
		public long Id { get; set; }
		public string UserId { get; set; }
		public string Username { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public short CountryId { get; set; }
		public short RoleId { get; set; }
		public short Rank { get; set; }
		public int Score { get; set; }
	}
}
