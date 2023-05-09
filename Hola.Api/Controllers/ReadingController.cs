using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models.Dic;
using Hola.Api.Models.Questions;
using Hola.Api.Service;
using Hola.Core.Helper;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Hola.Api.Requests.Reading;
using Hola.Core.DapperExtension;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Linq.Expressions;

namespace Hola.Api.Controllers
{
    public class ReadingController : ControllerBase
    {
        private readonly IReadingService _readingService;

        public ReadingController(IReadingService readingService)
        {
            _readingService = readingService;
        }

        // SEARCH
        [HttpPost("Search")]
        [Authorize]
        public async Task<JsonResponseModel> Search([FromBody] SearchReadingRequest model)
        {
            try
            {
                var search = model.Search;
                string title = search.GetValueByKey<string>("title");

                Func<Reading, bool> condition = x => x.IsDeleted == 0
                && (string.IsNullOrEmpty(title) ? true : x.Title.Contains(title));
                var list = _readingService.GetListPaged(model.PageIndex, model.PageSize, condition, "CreatedDate", false);
                return JsonResponseModel.Success(list);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
        }
    }
}
