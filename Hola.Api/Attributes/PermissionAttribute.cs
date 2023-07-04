using Hola.Api.Authorize;
using Hola.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hola.Api.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public PermissionKeyNames[] Permissions { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // -1 là quyền của BA Admin
            var permission = (context.HttpContext.HasPermission(Permissions));
            if (!permission)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            return;
        }
    }
}
