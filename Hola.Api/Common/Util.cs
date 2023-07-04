using Hola.Api.Authorize;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Common
{
    public static class Util
    {
        public static string ToPading(this string sqlcommand, int? currentPage, int? pageLimit)
        {
            string cmd = sqlcommand;
            var take = !pageLimit.HasValue || pageLimit.Value <= 0 ? "" : $"FETCH NEXT " + pageLimit.Value + " ROWS ONLY";
            cmd += " OFFSET " + ((currentPage ?? 1) - 1) * (pageLimit ?? 0) + " ROWS " + take;
            return cmd;
        }


        public static bool IsAuthenticated(this HttpContext httpContext) => httpContext?.User?.Identity?.IsAuthenticated ?? false;

        public static bool HasPermission(this HttpContext httpContext, PermissionKeyNames[] listPermission)
        {
            var permissionFromContext = httpContext?.User?.Claims
                .Where(x => x.Type == JwtClaimsTypes.Permission)
                .Select(x => x.Value).ToList();

            // Tất cả các quyền từ hệ thống đã định nghĩa trước
            var permissionFromInput = listPermission.Select(p => (int)p)?.ToList();

            // Nếu có quyền -1 tức là User Admin BA
            if (permissionFromContext.Contains("-1")) return true;
            if (permissionFromInput.Contains(0)) return true;
            if (permissionFromInput == null || permissionFromContext == null)
            {
                return false;
            }
            else if (permissionFromInput.Any(x => permissionFromContext.Contains(x.ToString())))// Nếu có bất kỳ quyền trong Input(của hàm) có trong Context (của User)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
