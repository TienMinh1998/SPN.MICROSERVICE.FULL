using Hola.Core.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
namespace Hola.Core.Middleware
{
    public class LockedTokensMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly Authorization.IActiveTokenHandler _activeTokenHandler;
        public LockedTokensMiddleWare(RequestDelegate next, Authorization.IActiveTokenHandler activeTokenHandler)
        {
            _next = next;
            _activeTokenHandler = activeTokenHandler;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {

                var useridclaim = context.User.Claims.FirstOrDefault(c => c.Type == Constants.UserIDClaimKey);
                if (useridclaim != null && !string.IsNullOrEmpty(useridclaim.Value))
                {
                    var userId = useridclaim.Value;
                    var tokenidclaim = context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
                    var adminModeClaim = context.User.Claims.FirstOrDefault(c => c.Type == Constants.TokenAdminMode);
                    if (adminModeClaim == null || !Convert.ToBoolean(adminModeClaim.Value))
                    {
                        if (tokenidclaim != null && !string.IsNullOrEmpty(tokenidclaim.Value))
                        {
                            var tokenId = Convert.ToInt64(tokenidclaim.Value);
                            var userActiveToken = await _activeTokenHandler.GetUserActiveToken(userId);
                            if (userActiveToken.HasValue)
                            {
                                if (userActiveToken != tokenId)
                                {
                                    context.Response.StatusCode = 200;
                                    context.Response.Headers.Add(Constants.ForceLogoutHeaderKey, "1");
                                    await context.Response.WriteAsync(string.Empty);
                                    return;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            await _next.Invoke(context);
        }
    }

    public static class LockedTokensMiddlewareExtensions
    {
        public static IApplicationBuilder UseLockedTokensMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LockedTokensMiddleWare>();
        }
    }
}
