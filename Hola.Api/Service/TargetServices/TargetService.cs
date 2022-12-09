using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.TargetRequests;
using Hola.Api.Models;
using Hola.Api.Service.BaseServices;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hola.Api.Service.TargetServices;


public class TargetService : BaseService, ITargetService
{

    private readonly IOptions<SettingModel> _options;
    private readonly string database = Constant.DEFAULT_DB;
    private string ConnectionString = string.Empty;
    public TargetService(IOptions<SettingModel> options) : base(options)
    {
        _options = options;
        ConnectionString = _options.Value.Connection + "Database=postgres";
    }


    /// <summary>
    /// Add Target
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public async Task<TargetModel> AddAsync(TargetModel target)
    {
        
        string query = "INSERT INTO qes.target (fk_userid, target_content, description, start_date, end_date, total_days,created_on) " +
            "VALUES({0}, '{1}', '{2}', '{3}', '{4}', {5}, now());";
        string sql_query = string.Format(query, target.FK_UserId,
            target.target_content, target.description,
            target.start_date, target.end_date,
            target.total_days);
            var result = await Excecute(ConnectionString, sql_query);
           return target;
    }

    /// <summary>
    /// Get Target By ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task<TargetModel> GetById(int Id,int userid)
    {
        string query = $"select * FROM qes.target where fk_userid ={userid} and pk_targetid ={Id}";
        var result = await FirstOrDefaultAsync<TargetModel>(ConnectionString, query);
        return result;
    }

    /// <summary>
    /// Get list target by UserID
    /// </summary>
    /// <param name="userid">int</param>
    /// <returns>Author : NGuyễn Viết Minh Tiến</returns>
    public async Task<List<TargetModel>> GetList(int userid)
    {
        string query = "SELECT pk_targetid, fk_userid, target_content, description, start_date, end_date, total_days, created_on FROM qes.target where fk_userid= {0}";
        var sql = string.Format(query, userid);
        var result = await QueryToListAsync<TargetModel>(ConnectionString, sql);
        return result;
    }


}
