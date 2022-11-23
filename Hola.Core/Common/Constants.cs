using System.Collections.Generic;

namespace Hola.Core.Common
{
    public static class Constants
    {
        internal static string UserIDClaimKey = "UserID";
        internal static string DeviceCodeClaimKey = "DeviceCode";
        internal static string UserAgentHeaderKey = "WP_UserAgent";
        internal static string TokenCreateDateClaimKey = "token.createdate";
        public static string ForceLogoutHeaderKey = "x-force-logout";
        public static string ForceLogoutPinVerifyKey = "PinVerifyForceLogout";
        public static string TokenAdminMode = "TokenAdminMode";
        internal static string DeviceIdClaimKey = "DeviceId";
        internal static string PhoneNumberClaimKey = "PhoneNumber";
        public const string SETTING_ACOUNT = "Account";
        public const string SETTING_TRANSACTION = "Transaction";

        public static class ApplicationName
        {
            public const string WeelPay = "WeelPay";
            public const string HolaPay = "HolaPay";
        }

        public static class Database
        {
            public static class OfferType
            {
                public const string Buy = "BUY";
                public const string Sell = "SELL";
                public const string Match = "MATCH";
            }

            public static class Otp
            {
                public const string Pending = "PENDING";
                public const string Resent = "RESENT";
                public const string Validated = "VALIDATED";
                public const string Success = "SUCCESS";
                public const string Expired = "EXPIRED";
            }

            public static class Country
            {
                public const short Korea = 1;
                public const short UnitedStates = 3;
                public const short Australia = 4;
                public const short Argentina = 5;
                public const short Armenia = 6;
                public const short Vietnam = 7;
            }

            public static class Currency
            {
                public const short Korea = 1;
                public const string KoreaSymbol = "₩";
            }

            public static class TransactionStatus
            {
                public const string WaitingForBankDetails = "WAITING_BANK_DETAILS";
                public const string Canceled = "CANCELED";
                public const string Pending = "PENDING";
                public const string Expired = "EXPIRED";
                public const string ExpiredApprove = "EXPIRED_APPROVE";

                public const string WantToSell = "WANT_SELL";

                public const string Transferred = "TRANSFERRED";
                public const string Approved = "APPROVED";
            }

            public static class WalletAction
            {
                public const string Freeze = "FREEZE";
                public const string Commit = "COMMIT";
                public const string Rollback = "ROLLBACK";
            }
        }

        public static class ServiceEndpoints
        {
            public const string Wallet = "Wallet";
            public const string Common = "Common";
            public const string Report = "Report";
            public const string Messaging = "Messaging";
            public const string VenlyAPI = "VenlyAPI";
        }

        public static class Request
        {
            public static class Headers
            {
                public const string UserId = "userId";
            }
        }

        public static class Deal
        {
            public static class Transaction
            {
                public static class Type
                {
                    public const string Buy = "buy_emoney";
                    public const string Sell = "sell_emoney";
                    public const string Match = "match_emoney";
                }

                public static class Status
                {
                    public const string WaitingBankDetails = "tran_waiting_bankdetails";
                    public const string Completed = "tran_completed";
                    public const string Pending = "tran_pending";
                    public const string PendingTransaction = "pending_transaction";
                    public const string Canceled = "tran_canceled";
                    public const string Transferred = "tran_transferred";
                    public const string Expired = "tran_expired";

                    public static string GetStatusTitle(string status)
                    {
                        string statusTitle = "WaitingBankDetails";
                        switch (status)//refacor/ from dictionary
                        {
                            case "tran_waiting_bankdetails": statusTitle = "Waiting for bank details"; break;
                            case "tran_completed": statusTitle = "Completed"; break;
                            case "tran_pending": statusTitle = "Waiting for transfer"; break;
                            case "tran_canceled": statusTitle = "Canceled"; break;
                            case "tran_transferred": statusTitle = "Waiting for approval"; break;
                            case "tran_expired": statusTitle = "Timeout"; break;
                            case "pending_transaction": statusTitle = "Pending Transaction"; break;
                        }

                        return statusTitle;
                    }
                }
            }
        }

        public static class CountryPhoneCodes
        {
            public const string Korea = "82";
            public const string USA = "1";
            public const string Australia = "61";
            public const string Argentina = "54";
            public const string Armenia = "374";

            public static List<string> AllPhoneCodes { get; set; } = new List<string>() { Korea, USA, Australia, Argentina, Armenia };
        }

        public static class Jwt
        {
            public const string UserIDClaimKey = "UserID";
        }

        public static class FireBase
        {
            public const string EventBalansCahged = "balanceChanged";
            public const string EventRecentActivityChanged = "recentActivityChanged";
            public const string EventNoteCount = "noteCount";
            public const string EventKYCStatusChange = "applyKYCStatusChanged";
            public const string EventSendCrypto = "eventSendCrypto";
            public const string EventReciverCrypto = "eventReciverCrypto";
            public const string EventAnnouncementCount = "announcementCount";
            public const string EventFeatureCount = "featureCount";
            public const string EventApplyStoreStatusChanged = "applyStoreStatusChanged";
            public const string EventApplyAgentStatusChanged = "applyAgentStatusChanged";
            public const string EventApplyAgentStatusApproved = "applyStatusApproved";
            public const string EventApplyAgentStatusDeclined = "applyStatusDeclined";
            public const string EventApplyAgentStatusUnderReview = "becomeAgentReview";

            public const string EventCurrencyChanged = "currencyChanged";
            public const string EventUserStatusUnlock = "userStatusUnlock";
            public const string EventUserStatusDeactive = "userStatusDeactive";
            public const string EventUserStatusReactive = "userStatusReactive";
            public const string p2pExchangeChanged = "p2pExchangeChanged";

        }

        public static class Client
        {
            public static class TransactionStatus
            {
                public const string WaitingForBankDetails = "Waiting for bank details";
                public const string Canceled = "Cancelled";
                public const string Pending = "Pending";
                public const string WantToSell = "Open";
                public const string Expired = "Expired";
                public const string Transferred = "Transferred To BankAccount";
                public const string Approved = "Completed";
            }

            public static class Actions
            {
                public const string Confirm = "confirm.bankaccount.transfer";
                public const string Transfer = "bankaccount.transfer";
                public const string ContactSupport = "contact.support";
                public const string ShowBankDetails = "show.bankaccount.details";
                public const string SendBankAccount = "send.bankaccount";
                public const string Cancel = "cancel";
                public const string ProceedLogout = "proceed.logout";
            }
        }
    }
}