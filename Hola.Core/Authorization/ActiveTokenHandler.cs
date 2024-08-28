using Hola.Core.Enums;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Hola.Core.Authorization
{
    public class ActiveTokenHandler : IActiveTokenHandler
    {
        private ConcurrentDictionary<string, long> _activeTokens = new ConcurrentDictionary<string, long>();

        private readonly ServiceClient _UserService;

        public ActiveTokenHandler()
        {
            _UserService = new ServiceClient(null, "User");
        }

        public async Task<long?> GetUserActiveToken(string userId)
        {
            if (_activeTokens.ContainsKey(userId))
                return _activeTokens[userId];
            var activeTokenId = await getUserActiveToken(userId);
            if (activeTokenId.HasValue)
            {
                _activeTokens[userId] = activeTokenId.Value;
                return activeTokenId.Value;
            }
            return null;
        }

        public void SetUserActiveToken(string userId, long tokenId)
        {
            _activeTokens[userId] = tokenId;
        }

        public async Task<long?> getUserActiveToken(string userId)
        {
            try
            {
                APICrossHelper aPICrossHelper = new APICrossHelper();

                var get = await aPICrossHelper.Get<JsonResponseModel>("https://holapayuser.axolotl18.com/Token/GetUserActiveToken?UserId=" + userId
                + "&LoginProvider=" + LoginProvider.ClientAPI);

                if (get != null && get.Status == 200)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<GetUserActiveTokenResponseCore>(get.Data.ToString()).TokenId;
                }
            }
            catch (Exception ex)
            {
                //TODO:write exception log here
            }
            return null;
        }

        public void RevokeUserActiveToken(string userId)
        {
            long tokenId;
            if (_activeTokens.ContainsKey(userId))
                _activeTokens.TryRemove(userId, out tokenId);
        }
    }
}