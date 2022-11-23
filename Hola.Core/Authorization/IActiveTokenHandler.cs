using System.Threading.Tasks;

namespace Hola.Core.Authorization
{
    public interface IActiveTokenHandler
    {
        void SetUserActiveToken(string userId, long tokenId);

        Task<long?> GetUserActiveToken(string userId);

        void RevokeUserActiveToken(string userId);
    }
}
