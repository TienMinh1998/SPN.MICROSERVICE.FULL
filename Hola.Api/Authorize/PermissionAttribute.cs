using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hola.Api.Authorize
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string permistion { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var checkPermission = context.HttpContext.HasPermission(permistion);
            if (!checkPermission)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
