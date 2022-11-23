using System;

namespace Hola.Core.Model.DBModel.usr
{
    public class UsersModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PinHash { get; set; }
        public string PinVersion { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordVersion { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public DateTime LastLoginAttemptDate { get; set; }
        public DateTime LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public bool PasswordEnabled { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string Email { get; set; }
        public int TwoFactorProvider { get; set; }
        public short CountryId { get; set; }
        public short LanguageId { get; set; }
        public long ImageId { get; set; }
    }
}
