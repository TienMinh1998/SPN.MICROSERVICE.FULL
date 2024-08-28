namespace Hola.Api.Common
{
    public static class SystemParam
    {
        public const string CLAIM_USER = "UserId";
    }

    public enum USERROLE : int
    {
        NORMAR_USER = 0,
        ADMIN
    }

    public enum PermissionKeyNames
    {
        READ = 1
    }
}
