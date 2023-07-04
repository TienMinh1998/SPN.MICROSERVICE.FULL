using Hola.Api.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hola.Api.Attributes
{
    public class AuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public AuthAttribute() : base()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.IsAuthenticated())
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
