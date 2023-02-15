using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Hola.Api.Authorize
{
    public static class HttpContextClaimsExtensions
    {
        public static bool HasPermission(this HttpContext httpContextcontext, string permission)
        {
            var permissionFromContext = httpContextcontext?.User?.Claims
              .Where(x => x.Type == JwtClaimsTypes.Permission)
              .Select(x => x.Value).ToList();
            string permissionInput = permission;

            if (permissionInput == null || permissionFromContext == null) return false;
            return true;
        }
    }
}
