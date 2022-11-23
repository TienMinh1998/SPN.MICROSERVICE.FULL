namespace Hola.Core.Common
{
    public static class ErrorCodes
    {
        #region not useed

        public const string AccessDenied = " Access.Denied";

        #endregion not useed

        #region internal error

        public const string NullRequest = "Null.Request";
        public const string ItemNotFound = "Item.NotFound";
        public const string ItemExists = "Item.Exists";
        public const string InvalidId = "Invalid.Id";
        public const string SQLError = "Sql.Error";
        public const string NullData = "null.data";

        public const string CannotDeleteOffer = "delete_offer_fail";
        public const string HasPendingBuy = "Has.Pending.Buy";
        public const string NoPendingBuyTransaction = "No.Pending.Buy.Transaction";
        public const string NoTransferredTransaction = "No.Transferred.Transaction";
        public const string NoWantToSellTransaction = "No.WantToSell.Transaction";
        public const string TransactionNotFound = "Transaction.NotFound";
        public const string TransactionCanNotBeCanceled = "Transaction.Cannotbe.Canceled";
        public const string TransactionAlreadyCanceld = "Transaction.Already.Canceld";
        public const string TransactionAlreadyExpired = "Transaction.Already.Expired";
        public const string MissingTransactionId = "Invalid.TransactionId";
        public const string MissingCurrencyId = "Invalid.CurrencyId";
        public const string MissingUserId = "UserId cannot be null!";
        public const string MissingImage = "Missing.Image";
        public const string MissingLanguageId = "Invalid.LanguageId";
        public const string MissingHtml = "Empty.Html";
        public const string MissingCategoryId = "Invalid.CategoryId";
        public const string MissingAmount = "Invalid.Amount";
        public const string AmountLesMin = "Amount.Less.MinAmount";
        public const string MissingBankAccountId = "Invalid.BankAccountId";
        public const string MissingCardHolderName = "Invalid.CardHolderName";
        public const string MissingCountryId = "Invalid.CountryId";
        public const string MismatchCountryId = "Mismatch.CountryId";
        public const string MismatchCurrencyId = "Mismatch.CurrencyId";
        public const string MissingABARouting = "Invalid.ABARouting";
        public const string MissingBSDBranch = "Invalid.BSDBranch";
        public const string MissingSwiftCode = "Invalid.SwiftCode";
        public const string MissingTaxID = "Invalid.TaxID";
        public const string MissingCBU = "Invalid.CBU";
        public const string InavlidStatus = "Invalid.Status";
        public const string InavlidStatusChange = "Invalid.Status.Change";
        public const string MissingFileName = "Missing.FileName";
        public const string MissingFileData = "Missing.FileData";
        public const string InvalidEmail = "Invalid.Email";
        public const string MissingPhoneNumber = "Missing.PhoneNumber";
        public const string MissingOtpCode = "Missing.OtpCode";
        public const string SendSmsError = "Send.Sms.Error";
        public const string NotificationNotFoundWithKey = "Notification.NotFound.WithKey";
        public const string NotificationRecipientNotFound = "Notification.Recipient.NotFound";
        public const string FireBaseTokenNotFound = "FireBase.Token.NotFound";
        public const string TitleEmpty = "title.empty";
        public const string HasRelatedStores = "has.related.stores";
        public const string HasRelatedProducts = "has.related.products";
        public const string HasRelatedAnnouncements = "has.related.announcements";
        public const string InvalidStoreId = "Invalid.StoreId";
        public const string InvalidStoreLogoId = "Invalid.StoreLogoId";
        public const string InvalidWorkdayTime = "Invalid.time.0<=time<=1439";
        public const string InvalidWorkdayDay = "Invalid.time.1<=day<=7";
        public const string AmountZero = "Amount.Zero";
        public const string StartDateRequie = "Start date can not greater than End date!";
        public const string MaxAmountZero = "The Max Amount is Zero";
        public const string MinAmountZero = "The Min Amount is Zero";
        public const string MinMaxAmount = "The Min Amount less than Max Amount";
        public const string CurrencyFrom = "CurrencyIdFrom is Zero";
        public const string CurrencyTo = "CurrencyIdTo is Zero";
        public const string RateFrom = "RateFrom is Zero";
        public const string RateTo = "RateTo is Zero";
        public const string MissingProductPrice = "Invalid.ProductPrice";
        public const string InvalidProductId = "Invalid.ProductId";
        public const string LanguageKeyExists = "LanguageKey.Exists";
        public const string InvalidPercentage = "Invalid.Percent";
        public const string InvalidUserRole = "Invalid.Role";
        public const string InvalidDateRange = "Invalid.DateRange";
        public const string ApplyStoreAlreadyExist = "already.applyed";
        public const string AlreadyStoreMember = "already.store.member";
        public const string StoreHasAppliedUser = "store.has.already.applyed.user";
        public const string PromotionAlreadyExists = "already.exists.promotion.in.given.daterange";
        public const string MaxFileSizeExceeded = "file.size.exceeds.maxsize";
        public const string WrongFileExtension = "not.supported.file.extension";
        public const string InvalidCurrencyId = "Invalid.CryptoCurrency.Id";
        public const string NoWalletsForUser = "No.Wallets.For.User";
        public const string InvalidCryptoWalletId = "Invalid.Crypto.Wallet.Id";
        public const string InvalidCryptoWalletTransactionId = "Invalid.Crypto.Wallet.Transaction.Id";
        public const string CryptoWalletTransactionNotPending = "Crypto.Wallet.Transaction.Not.Pending";
        public const string InvalidDestinationAddress = "Invalid.Destination.Address";
        public const string InvalidExpirationDate = "Invalid.Expiration.Date";
        public const string InvalidMinAmount = "Invalid.Min.Amount";
        public const string InvalidAmount = "Invalid.Amount";
        public const string InvalidBankAccount = "Invalid.BankAccount";
        public const string UserBlocked = "User.Blocked";
        public const string InvalidAddress = "Invalid.Address";
        public const string MissingBlockchainName = "Missing.Blockchain.Name";
        public const string MissingCurrencyCode = "Missing.Currency.Code";
        public const string MissingCurrencyLogo = "Missing.Currency.Logo";
        public const string CryptoCurrencyAlreadyExist = "Crypto.Currency.Already.Exist";
        public const string OnlyNormalUserCanBeGranted = "Only.Normal.User.CanBe.Granted";
        public const string OnlySuperAdminCanGrantAdminOrSuperAdmin = "only.superadmin.can.grant.admin.or.superadmin";
        public const string UserCanNotDirectTransferHimself = "user.can.not.direct.transfer.himself";

        #endregion internal error

        #region language dictionary

        public const string BuyerNotFound = "Buyer.NotFound";//dont change this key text
        public const string SellerNotFound = "Seller.NotFound";//dont change this key text
        public const string OfferOpentTransaction = "Offer.Has.Opened.Transaction";//missing in dictionnary
        public const string ExpiredTransactionNotFound = "session_expired";//dont change this key text
        public const string AlredyHasCardNumber = "Already.Has.CardNumber";// dont change this key text
        public const string OtpCodeNotFound = "invalid_code";// dont change this key text
        public const string OtpCodeExpired = "OtpCode.Expired";//missing in dictionnary
        public const string LoginNotFound = "login.notfound";
        public const string AdminLoginPwdAlreadyCreated = "login.password.created";
        public const string PwdNotMeetRequirements = "not.meet.requirements";
        public const string UserLocked = "user.locked";
        public const string UserLockedEnterWrongPin = "user.locked.enter.wrong.pin";
        public const string UserNotActive = "user.not.active";
        public const string UserNotDeactivated = "user.not.deactivated";
        public const string UserNotLocked = "user.not.locked";
        public const string PinIncorrect = "pin.incorrect";
        public const string OldPwdIncorrect = "old.password.incorrect";
        public const string OldNewPwdAreEquals = "old.password.equal.to.new.password";
        public const string UnAuthorizedLogin = "login.unAuthorized";
        public const string AdminPwdIncorrect = "password.incorrect";
        public const string AdminPwdCreateNotAgency = "login.notAgency";
        public const string AdminAgency2FAOTPInvalid = "2faotp.invalid";
        public const string TwoFAOTPInvalid = "2faotp.invalid";
        public const string InternalServerError = "internal.error";
        public const string DestinationNotGrantedUser = "destination.not.granted.user";

        #endregion language dictionary

        public const string m_login_2faotp_empty = "login.2faotp.empty";
        public const string CurrencyUnits = "Units not be null and must be a number";
        public const string CurrencyNumber = "Number must be less than 4 characters";
        public const string CurrencyCode = "Code must be less than 4 characters and not be null";
        public const string CurrencySymbol = "Symbol must be less than 3 characters and not be null";
        public const string CurrencyMinAmount = "MinAmount must be greater than 0";
        public const string CodeSymbolExist = "Code or Symbol has exist";
        public const string AlredyHasKoreanCurrency = "Already.Has.Korean.Currency";
        public const string OnlyAgentCanCreate = "only.agent.can.create";
        public const string CanNotDeleteKoreanBankAccount = "can.not.delete.korean.bank.account";
        public const string CanNotDeleteBankAccount = "can.not.delete.bank.account";

        public const string m_register_admin_phonenumber_empty = "register.admin.phonenumber.empty";

        public const string WalletFreezeCallError = "Wallet.Freeze.APIcall.Error";
        public const string WalletFreezeNullResponse = "Wallet.Freeze.APIcall.NullResponse";
        public const string WalletRollbackCallError = "Wallet.Rollback.APIcall.Error";
        public const string WalletRollbackNullResponse = "Wallet.Rollback.APIcall.NullResponse";
        public const string WalletCommitCallError = "Wallet.Commit.APIcall.Error";
        public const string WalletCommitNullResponse = "Wallet.Commit.APIcall.NullResponse";

        public const string wlt_Missing = "wlt.missing";
        public const string wlt_NegativeBalance = "wlt.negative.balance";
        public const string wlt_NegativeBlockedBalance = "wlt.negative.blockedbalance";
        public const string wlt_AlreadyExist = "wlt.already.exist";
        public const string wlt_InvalidData = "invalid.wallet.data";
        public const string wlt_InvalidCurrencyId = "wlt.invalid.currency";
        public const string wlt_InvalidBounds = "wlt.invalid.bounds";

        public const string usr_existingUser = "user.already.exist";
        public const string usr_score_update_failed = "user.score.update.failed";

        public const string tkn_invalid = "token.invalid";
        public const string tkn_obsolete = "token.obsolete";
        public const string tkn_invalid_origin = "token.invalid.origin";
        public const string tkn_old_version = "token.old.version";
        public const string tkn_expired = "token.expired";

        public const string qr_invalid = "qr.invalid";
        public const string otp_wrong = "otp.wrong";
        public const string usr_Already_Logged_In = "user.already.logged.in";

        //Model State
        public const string ms_negative_amount = "ms.negative.balance";

        public const string ms_empty_amount = "amount.empty";
        public const string ms_destId_empty = "destination.userid.empty";
        public const string ms_sourceId_empty = "source.walletid.empty";
        public const string ms_otp_empty = "otp.empty";

        //Exchange Currency Wallet
        public const string xchg_DestUser_ToWallet_NotExist = "xchg.dest.towallet.notexist";

        public const string xchg_DestUser_ToWallet_NotEnoughFunds = "xchg.dest.towallet.not.enough.funds";
        public const string User_not_found = "user.not.found";
        public const string BalanceBigger = "amount.for.transfer.bigger";
        public const string BuyAmountLessThan = "amount.for.buy.less";
    }
}