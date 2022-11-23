using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Helper
{
    public static class ClaimExtensions
    {
        public static T TryGetValue<T>(this Claim claim) where T : IConvertible
        {
            try
            {
                Type type = typeof(T);
                Type uType = Nullable.GetUnderlyingType(type);

                if (uType != null)
                {
                    return (claim.Value == null) ? default : (T)Convert.ChangeType(claim.Value, uType);
                }
                else
                {
                    return (T)Convert.ChangeType(claim.Value, type);
                }
            }
            catch
            {
                return default;
            }
        }

        public static T GetClaim<T>(this IEnumerable<Claim> claims, string claimKey) where T : IConvertible
        {
            var claim = claims.FirstOrDefault(c => c.Type == claimKey);

            if (string.IsNullOrEmpty(claim.Value))
                return default;

            return claim.TryGetValue<T>();
        }
    }
}
