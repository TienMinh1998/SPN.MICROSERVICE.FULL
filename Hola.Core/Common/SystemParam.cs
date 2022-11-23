namespace Hola.Core.Common
{
    public static class SystemParam
    {
        /// <summary>
        /// this is ErrorCode when Process Fail
        /// </summary>
        public static int ACTIVE_WALLET_NUMBER = 0;
        public const  int ERRORCODE = 100;
        public const int DEFAULTTIMEOUTCACHE = 100;
        public const int LONGTIME_CACHE = 5000;
        public const short ISACTIVE = 1;
        public const short NONACTIVE = 0;
        /// <summary>
        ///  This is Success whern Process Success
        /// </summary>
        public const int SUCCESSCODE = 200;

        public const string MSG_SUCCESS = "SUCCESS";
        public const string Cancel_Offer_bull = "DONT HAVE ANY OFFER IN THIS USER";
        public const string MSG_SERVER_ERROR = "SERVER ERROR";
        public const string NOT_FOUND_RECORD = "NOT FOUND RECORD";
        public const string WAIT_ADMIN_COMMIT = "request has been sent before, please wait for admin to confirm";
        public const string SENT_SUCCESS = "Successfully sent";
        public const string DATA_NOTNULL = "Data can't Null";
        public const string WALLET_AVAILABLE = "WALLET already exists";
        public const string WALLET_ERROR = "Error to get list item";

        public const string CONNECTION_FAIL = "check the connection to the server ";
        public const string UPDATE_SUCCESS = "Update SUCCESS";


        public const int PAGE_SIZE_DEFAULT = 10;
        public const int PAGE_NUMBER_DEFAULT = 1;


        public const int SERVER_ERROR_CODE = 501;
        public const int NOT_FOUND_RECORD_CODE = -1;
        public const int SUCCESS_CODE = 200;
        public const int NOT_FOUND = 404;
        public const int BAD_REQUEST_CODE = 400;
        public const int VALUE_ZEZO = 0;
        public const int VALUE_ONE = 1;

        //KYC
        public const short ISKYC = 1;
        public const short NOTKYC = 0;


        public const string FILE_NAME = "Files";


        public const int ZERO_VALUES = 0;
        public const int ACTIVE = 1;
        public const int PENDING = 0;
        public const string UPDATE_ERROR = "Can't not Update";

        // KYC Status 
        public const int KYC_waitingForKYC = 0;
        public const int KYC_APPLIED = 1;
        public const int KYC_PENDING = 2;
        public const int KYC_APPROVED = 3;
        public const int KYC_REJECTED = 4;
        public const int KYC_BACKLISTED = 5;

        // Status của từng bước 
        public const int KYC_ITEM_REJECT = 0;
        public const int KYC_ITEM_PENDING = 1;
        public const int KYC_ITEM_APPROVED = 2;
        public const int KYC_ITEM_FOR_REVIEW = 3;
        public const int KYC_ITEM_WAITING = 4;

        // 5 Step Type : 
        public const string BankInfotable = "BankInfomation";
        public const string Declaration = "DeclarationInfomation";
        public const string IdentityDocument = "IdentityDocument";
        public const string Address = "UserAddressInfomation";
        public const string UserInfomation = "UserInfomation";

        // Wallet
        /// <summary>
        /// Wallet that can be recovered in case the pin is lost
        /// </summary>
        public const string WHITE_LABEL = "WHITE_LABEL";
        public const string KYC_STEP = "General KYC";
        // NOTE
        public const string NOTI_COMPLETE_5_STEP = "You must first complete 5 steps";
        // VENLY

        public const string CANT_NOT_CREATE_TRANFER_FROM_VENLY = "Can't not execute-transfer";
        public const string INVALID_COINNAME = "Invalid coin's name, please check 'secretType' again";
        public const int UPDATE_TIME_LISTCOIN = 3;
        public const string GUID_NULL = "00000000-0000-0000-0000-000000000000";
        public const bool SUCCESS_FROM_VENLY = true;
        public const int TYPE_P2P_BASIC = 1;
        public const int TYPE_P2P_CRYPTO = 2;
        public const string LIMIT_REQUEST_TRANSFER = "Only 1 transaction can be made in 10 minutes";
        public const string ERROR_INVALID_PIN = "";
        public const string DONE = "DONE";
        // TYPE P2p
        public const string EMONEY_TO_EMONEY = "EMONEY";
        public const string CRYPTO_TO_CRYPTO = "CRYPTO";
        public const string CRYPTO_TO_EMONEY = "CRYPTO-EMONEY";
        public const string EMONEY_TO_CRYPTO = "EMONEY-CRYPTO";

    }
}
