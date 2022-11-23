namespace Hola.Core.Common
{
    public static class Constant
    {
        public const string ACCCOUNT_DB = "account_db";
        public const string  COMMOND_DB = "commond_db";
        public const string TRANSACTION_DB = "transaction_db";
        public const string MESSAGE_DB = "Message_db";
        public const string DEFAULT_DB = "postgres";
        public const int ZEZO = 0;
        /// <summary>
        /// MSG for CRUD & Log
        /// </summary>
        public const string MSG_SUCCESS = "Success";
        public const string REQUEST_APPLY_SUCCESS = "SENT REQUEST";
        public const string MSG_ERROR = "Error";
        public const string LANGUAGE_AVAILABLE = "Sory! language available, Try again";
        public const string MSG_DATA_NOT_FOUND = "Data not found";
        public const string MSG_UPDATE_FALSE = "Cannot update data!";
        public const string MSG_DELETE_FALSE = "Cannot delete data!";
        public const string MSG_SERVER_ERROR = "Server Error";

        public const string IP_DOCKER = "172.17.0.1";
        public const string IP_ADDRESS_LOCAL = "127.0.0.1";
        #region HTTP CODE
        public const int SUCCESS_CODE = 200;
        public const int SERVER_ERROR_CODE = 404;

        #endregion

        public const int PAGE_NUMBER_DEFAULT = 1;
        public const int PAGE_SIZE_DEFAULT = 10;


        public const string REQUEST_ADDLY_SENDBEFORE = "Request has been sent before. please wait for admin to handle!";
    }
}