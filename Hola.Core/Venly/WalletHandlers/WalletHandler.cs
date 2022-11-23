
using Hola.Core.Helper;
using Hola.Core.Venly.Base;
using Hola.Core.Venly.Model.Request;
using Hola.Core.Venly.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Venly.Model;
using Venly.Model.Requests;

namespace Hola.Core.Venly.WalletHandlers
{
    public class WalletHandler  : VenlyHttpClient {
        public WalletHandler(string accessToken, VenlyConfiguration venlyConfiguration) : base(accessToken, venlyConfiguration)
        {
        }

        /// <summary>
        /// Address : https://api-wallet-staging.venly.io/api/wallets<T.Criss>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<VenlyResponseBase<Wallet>?> CreateWalletAsync(CreateWalletRequest request)
        {
            return await PostAsync<Wallet>(_venlyConfiguration.WalletEndPoint + "wallets", JsonConvert.SerializeObject(request));
        }

        /// <summary>
        /// Get WalletId 
        /// </summary>
        /// <param name="walletID"></param>
        /// <returns></returns>
        public async Task<VenlyResponseBase<Wallet>> GetWalletByWalletID(string walletID)
        {
            var a = BaseAddress + $"wallets/{walletID}";
            return await GetAsync<Wallet>(a);
        }
        public async Task<VenlyResponseBase<Wallet>> ChangePinCode(string walletID,ChangePinCodeRequest changePinCodeRequest)
        {
            var url = BaseAddress + $"wallets/{walletID}/security";
            return await PatchAsync<Wallet>(url, JsonConvert.SerializeObject(changePinCodeRequest));
        }
        public async Task<ChainsResponse> Get_Chains()
        {
           
            return await GetAsyncT<ChainsResponse>(_venlyConfiguration.WalletEndPoint + $"chains");
        }

        public async Task<UserBalanceReponse> Get_User_Balance()
        {
            return await GetAsyncT<UserBalanceReponse>(_venlyConfiguration.MarketEndPoint +$"user/credits");
        }
        /// <summary>
        /// The destination of a transfer is not limited to a blockchain address, we also support email addresses.
        /// </summary>
        /// <returns></returns>
        public async Task<SendCryptoResponse> TransferaNativeToken(TranferRequest tranferRequest)
        {
            return await PostAsyncGeneric<SendCryptoResponse>(_venlyConfiguration.WalletEndPoint + "transactions/execute", JsonConvert.SerializeObject(tranferRequest));
        }

        public async Task<WalletAddressResponse> GetWalletInfoByAddress(string address, string secretType)
        {
            var a = BaseAddress + $"wallets/{secretType}/{address}/nonfungibles";
            return await GetAsyncT<WalletAddressResponse>(a);
        }
        public async Task<VenlyWalletResponse> Wallets()
        {
            return await GetAsyncT<VenlyWalletResponse>(_venlyConfiguration.WalletEndPoint + $"wallets");
        }


     
    }
}
