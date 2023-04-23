using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests;
using EntitiesCommon.Requests.TargetRequests;
using Hola.Api.Common;
using Hola.Api.Models.Categories;
using Hola.Api.Requests.Users;
using Hola.Api.Service;
using Hola.Api.Service.TargetServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class TartgetController : ControllerBase
    {
        private readonly ITargetService _targetService;
        private readonly IMapper _mapper;
        public TartgetController(ITargetService targetService, IMapper mapper)
        {
            _targetService = targetService;
            _mapper = mapper;
        }



        /// <summary>
        /// Add new a target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [HttpPost("AddTarget")]
        [Authorize]
        public async Task<JsonResponseModel> AddTarget([FromBody] AddTargetRequest target)
        {
            int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
            var entity = _mapper.Map<Target>(target);
            entity.FK_UserId = userid;
            entity.created_on = DateTime.Now;
            entity.total_days = TotalDate(target.start_date, target.end_date);
            var response = await _targetService.AddAsync(entity);
            return JsonResponseModel.Success(response);
        }


        /// <summary>
        /// Get list Target By UserID
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [HttpGet("Targets")]
        [Authorize]
        public async Task<JsonResponseModel> Targets()
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
                var response = await _targetService.GetAllAsync(x => x.FK_UserId == userid);
                if (response == null || response.ToList().Count() <= 0)
                    return JsonResponseModel.Error("Danh sách rỗng", 199);
                return JsonResponseModel.Success(response);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);

            }

        }



        private int TotalDate(DateTime start_date, DateTime end_Date)
        {
            if (start_date.Date == end_Date.Date) return 1;
            if (start_date.Date < end_Date.Date)
            {
                TimeSpan time = end_Date - start_date;
                return time.Days;
            }




            return 0;
        }

    }
}
