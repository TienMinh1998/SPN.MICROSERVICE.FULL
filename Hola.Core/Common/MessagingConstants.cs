using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Common
{
    public static class MessagingConstants
    {
        public static class Texts
        {
            public const string DirectTransferTo = "DIRECT_TO";
            public const string DirectTransferFrom = "DIRECT_FROM";
            public const string AdminAdded = "ADMIN_ADDED";
            public const string AdminDeducted = "ADMIN_DEDUCTED";
            public const string BecomeAgent = "BECOME_AGENT";
            public const string KycApplied = "KYC_APPLIED";
            public const string KycStatusChange = "KYC_STATUS_UPDATE";
            public const string BuyWarning = "BUY_WARNING";

            public const string SellerOfferMatchedUser = "SELL_OFFER_MATCH_USER";
            public const string SellerOfferMatchedAgent = "SELL_OFFER_MATCH_AGENT";
            public const string SellerOfferBankDetailsUser = "SELL_OFFER_BANK_DETAILS_USER";

            public const string BuyerOfferMatchedUser = "BUY_OFFER_MATCH_USER";
            public const string BuyerOfferMatchedAgent = "BUY_OFFER_MATCH_AGENT";

            public const string AutoMatchedBuyer = "AUTOMATCH_BUYER";
            public const string AutoMatchedSeller = "AUTOMATCH_SELLER";
            public const string TransferBuyer = "AUTOMATCH_TRANSFER_BUYER";
            public const string TransferSeller = "AUTOMATCH_TRANSFER_SELLER";

            public const string ExchangeSuccess = "EXCHANGE_SUCCESS";
            public const string ExchangeFail = "EXCHANGE_FAIL";

            public const string TransactionCompleted = "TRAN_COMPLETED";
            public const string TransactionCancelled = "TRAN_CANCELLED";
            public const string TransactionExpired = "TRAN_EXPIRED";

        }

        public static class Types
        {
            public const string NoteTransfer = "note_transfer";
            public const string NoteBuy = "note_buy";
            public const string NoteSell = "note_sell";
            public const string NoteAgent = "note_agent";
            public const string NoteKYCStatusChange = "note_kyc_status_change";
            public const string NoteWarning = "note_warning";
            public const string NoteKYC = "note_kyc";
            public const string NoteP2P = "note_p2p";
        }
        public static class TransactionTypes
        {
            public const string Direct = "direct";
            public const string Offer = "offer";
            public const string P2P = "p2p";
            public const string Admin = "admin";
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
