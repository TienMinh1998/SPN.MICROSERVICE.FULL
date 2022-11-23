using Hola.Core.Venly.Base;
using Hola.Core.Venly.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venly.Model;

namespace Hola.Core.Venly.NFTHandlers
{
    public class NFTHandler : VenlyHttpClient
    {
        public NFTHandler(string accessToken, VenlyConfiguration venlyConfiguration) : base(accessToken, venlyConfiguration)
        {
        }
        public async Task<SupportedChainsForItemCreation> Get_Chains()
        {
            return await GetAsyncT<SupportedChainsForItemCreation>(_venlyConfiguration.NFTEndPoint + $"env");
        }
    }
}
