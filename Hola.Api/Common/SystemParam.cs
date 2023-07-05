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
        AddVocabulary = 1,


        // Reading
        ReadingView = 2,
        ReadingDelete = 3,
        ReadingEdit = 4,
        ReadingAdd = 5,

        // News
        NewsAdding = 6
    }
}
