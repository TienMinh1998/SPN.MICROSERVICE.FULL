using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Common
{
    public class SQLCommandText
    {
        /// <summary>
        /// Search for Client
        /// </summary>
        public const string SQL_SEARCH_OFFER_APP = "SELECT* FROM deal.Get_Offer_Search_Model('_createdId');";
        /// <summary>
        /// Search for Admin
        /// </summary>
        public const string SQL_SEARCH_OFFER_ADMIN = "select* From  deal.get_offer_search_model_by_admin('')";
        /// <summary>
        /// DetailSQL command text
        /// </summary>
        public const string SQL_MULTIPLE_KYC_DETAIL = " SELECT  \"KycStage\",\"Status\", \"CreatedDate\", \"Note\"  FROM kyc.\"KYCs\" WHERE \"KycId\" = '@Id' AND \"IsActive\" = Cast(1 as bit);" +
                                         " SELECT  \"UserId\", \"KycId\", \"Status\", \"AccountHolderName\", \"BankName\", \"BankCountryDetail\", \"BankBranchName\", \"AccountNumber\"," +
                                         " \"BankCountryStatement\", \"BankStatement\", \"EmploymentStatus\", " +
                                         "\"NetWorthUsDollars\", \"InfoOnSourceAndUseOfFunds\", \"Note\", \"IsActive\" FROM kyc.\"KYCBankInfo\" WHERE \"KycId\" = '@Id' AND \"IsActive\" = Cast(1 as bit); "
                                          + " SELECT \"Id\", \"UserId\", \"KycId\", \"Status\", \"DocumentType\", \"IDCardBack\"," +
                                         " \"IDCardFont\", \"ImageId\", \"Note\", \"IsActive\" FROM kyc.\"KYCDocs\" WHERE \"KycId\" = '@Id' AND \"IsActive\" = Cast(1 as bit); " +
                                         "SELECT  \"Status\", \"UserId\", \"KycId\",\"AttachAfterSigning\", \"Note\", \"IsActive\"" +
                                         " FROM kyc.\"KYCDeclarations\" WHERE \"KycId\" = '@Id' AND \"IsActive\" = Cast(1 as bit); " +
                                         "SELECT \"Id\", \"UserId\", \"KycId\", \"Status\", \"CountryId\", \"State\", \"City\", \"District\"," +
                                         " \"BuildingName\", \"StreetName\", \"ZipCode\", \"FileId\", \"Note\", \"IsActive\" FROM kyc.\"KYCUserAddress\" WHERE \"KycId\" = '@Id' AND \"IsActive\" = Cast(1 as bit); " +
                                        "SELECT \"Status\", \"FirstName\", \"LastName\", \"DateOfBirth\", \"Nationality\", \"Gender\", \"CountryCode\", " +
                                        "\"PhoneNumber\", \"Email\", \"CreatedDate\", \"ModifyDate\", \"Note\", \"IsActive\",\"RejectReason\" FROM kyc.\"KYCUserInfo\"  WHERE \"KycId\" = '@Id' AND \"IsActive\" = Cast(1 as bit);";

        public const string SQL_SELECT_LIST_P2P = " SELECT  exo.\"Id\" as  \"id\",exo.\"Status\" as  \"status\",  exo.\"StartDate\" as \"startDate\",  exo.\"EndDate\" as  \"endDate\",  exo.\"TotalAmount\" as \"totalSaleAmount\", " +
            " exo.\"MinAmount\" as \"minimumOfferAmount\" ,  exo.\"CreatedDate\" as \"createdDate\",  exo.\"CreatedBy\" as \"createdBy\",  exo.\"ModifiedDate\" as \"modifiedDate\"," +
                    "  exo.\"MaxAmount\" as \"maximumOfferAmount\", exo.\"Type\" as \"typeId\", exo.\"CurrencyIdFrom\" as \"currencyIdFrom\" , exo.\"CurrencyIdTo\" as \"currencyIdTo\"," +
                    " exo.\"RateFrom\" as \"rateFrom\" , exo.\"RateTo\" as \"rateTo\",  exo.\"Remains\" as \"remains\", exr.\"Commission\" as \"commission\",exo.\"SecretTypeTo\" as \"secretTypeTo\",exo.\"SecretTypeFrom\" as \"secretTypeFrom\"  " +
            "FROM p2p.\"ExchangeOffer\" as exo LEFT JOIN p2p.\"ExchangeRate\" as exr on exo.\"RateId\" = exr.\"Id\" LEFT JOIN p2p.\"AgentRank\" ar on exo.\"UserId\" = ar.\"AgentId\" WHERE exo.\"IsDeleted\" = 'false'";

        public const string SQL_SELECT_LIST_P2P_WITH_STARTDATE = " SELECT  exo.\"Id\" as  \"id\", exo.\"Status\" as  \"status\",  exo.\"StartDate\" as \"startDate\",  exo.\"EndDate\" as  \"endDate\",  exo.\"TotalAmount\" as \"totalSaleAmount\", " +
    " exo.\"MinAmount\" as \"minimumOfferAmount\" ,  exo.\"CreatedDate\" as \"createdDate\",  exo.\"CreatedBy\" as \"createdBy\",  exo.\"ModifiedDate\" as \"modifiedDate\"," +
            "  exo.\"MaxAmount\" as \"maximumOfferAmount\", exo.\"Type\" as \"typeId\", exo.\"CurrencyIdFrom\" as \"currencyIdFrom\" , exo.\"CurrencyIdTo\" as \"currencyIdTo\"," +
            " exo.\"RateFrom\" as \"rateFrom\" , exo.\"RateTo\" as \"rateTo\",  exo.\"Remains\" as \"remains\", exr.\"Commission\" as \"commission\", exo.\"SecretTypeFrom\" as \"secretTypeFrom\", exo.\"SecretTypeTo\" as \"secretTypeTo\" " +
    "FROM p2p.\"ExchangeOffer\" as exo LEFT JOIN p2p.\"ExchangeRate\" as exr on exo.\"RateId\" = exr.\"Id\" LEFT JOIN p2p.\"AgentRank\" ar on exo.\"UserId\" = ar.\"AgentId\" WHERE exo.\"IsDeleted\" = 'false' AND exo.\"StartDate\" <= now()";

        public const string SQL_GET_WALLET_BY_ID = "SELECT wc.\"id\",wc.\"VenlyBalance\", wc.\"address\", wc.\"walletType\", cc.\"SecretType\",cc.\"Logo\", wc.\"createdAt\", wc.\"archived\", wc.\"description\" " +
                    ", wc.\"primary\", wc.\"hasCustomPin\", wc.\"identifier\", wc.\"available\", wc.\"balance\", wc.\"gasBalance\", cc.\"Symbol\", wc.\"gasSymbol\" " +
                    ", wc.\"rawBalance\", wc.\"rawGasBalance\", wc.\"decimals\", wc.\"isDeleted\",cc.\"Status\",cc.\"USD\",cc.\"KRW\" " +
                    "FROM wlt.\"WalletCrypto\" wc left join wlt.\"CryptoCurrency\" cc On wc.\"secretType\" = cc.\"SecretType\" WHERE \"id\" = '@walletId@' and \"isDeleted\"= '0' ";

        public const string SQL_GET_HISTORY_SEND = " SELECT ct.\"CreatedBy\" as \"UserId\", wc.\"symbol\" as \"CryptoType\", " +
                "ct.\"InputAmount\" as \"TotalSend\",  ct.\"Id\" as \"TXID\",  ct.\"FromSecretType\" as \"CryptoName\"," +
                " wc.\"balance\" as \"Amount\", wc.\"address\" as \"SenderAddress\", ct.\"DestAddress\" as \"ReceiverAddress\"," +
                " ct.\"CreatedDate\" as \"Date\", ct.\"Status\" FROM wlt.\"CryptoTransactions\" as \"ct\" " +
                "INNER JOIN wlt.\"WalletCrypto\" as \"wc\" ON ct.\"WalletId\" = wc.\"id\" ";
        public const string SQL_GET_HISTORY_SEND_COUNT = " SELECT COUNT(1) FROM wlt.\"CryptoTransactions\" as \"ct\" " +
                "INNER JOIN wlt.\"WalletCrypto\" as \"wc\" ON ct.\"WalletId\" = wc.\"id\" ";

        public const string SQL_GET_HISTORY_RECIVER = " SELECT wc.\"UserId\" as \"UserId\", wc.\"symbol\" as \"CryptoType\", " +
               "ct.\"InputAmount\" as \"TotalSend\",  ct.\"Id\" as \"TXID\",  ct.\"FromSecretType\" as \"CryptoName\"," +
               " wc.\"balance\" as \"Amount\", wc.\"address\" as \"SenderAddress\", ct.\"DestAddress\" as \"ReceiverAddress\"," +
               " ct.\"CreatedDate\" as \"Date\", ct.\"Status\" FROM wlt.\"CryptoTransactions\" as \"ct\" " +
               "INNER JOIN wlt.\"WalletCrypto\" as \"wc\" ON ct.\"DestAddress\" = wc.\"address\" ";

        public const string SQL_GET_HISTORY_RECIVER_COUNT = " SELECT COUNT(1) FROM wlt.\"CryptoTransactions\" as \"ct\" " +
                "INNER JOIN wlt.\"WalletCrypto\" as \"wc\" ON ct.\"DestAddress\" = wc.\"address\" ";

    }
}
