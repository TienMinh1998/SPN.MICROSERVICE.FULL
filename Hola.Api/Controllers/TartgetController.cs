using AutoMapper;
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
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Get List Target
        /// </summary>
        /// <returns></returns>
        [HttpPost("Targets")]
        [Authorize]
        public async Task<JsonResponseModel> GetTargets([FromBody] PaddingRequest request)
        {
            string userid = User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value;
            var result = new Dictionary<string, object>();
            var targets = await _targetService.GetList(int.Parse(userid));
            result.Add("userid", userid);
            result.Add("targets", targets);
            return JsonResponseModel.Success(result,"Add Target Successful");
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
            var entity = _mapper.Map<TargetModel>(target);

            entity.FK_UserId = userid;
            var response = await _targetService.AddAsync(entity);
            return JsonResponseModel.Success(response);

        }

        /// <summary>
        /// Get target by target ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetTargetById/{Id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetTargetById(int Id)
        {
            int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
            var response = await _targetService.GetById(userid, Id);
            if (response != null)
                return JsonResponseModel.Success(response); 
                return JsonResponseModel.Error("Not found Target! Please Check your targetID", 404);




        }
    }
}
