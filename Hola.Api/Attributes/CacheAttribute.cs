using Hola.Api.Configurations;
using Hola.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hola.Core.Model;
using System.IO;
using Hola.Core.Common;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Hola.Core.Helper;

namespace Hola.Api.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// unit timeout : Seconds
        /// </summary>
        private readonly int _timeout;

        private readonly string _key = "HGAS>?aS";
        public CacheAttribute(int timeout = 100)
        {
            _timeout = timeout;
        }
        /// <summary>
        /// check cache available 
        /// Nếu có rồi lấy cache ra, nếu chưa có thì setcache
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!config.Enabled)
            {
                await next();
                return;
            }

            var caheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = await Extensions.GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await caheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            var dataResult = executedContext.Result as ObjectResult;
            if (((JsonResponseModel)dataResult.Value).Status == 200)
                await caheService.SetCacheAsync(cacheKey, dataResult.Value, System.TimeSpan.FromSeconds(_timeout));
        }

      

    }
}
