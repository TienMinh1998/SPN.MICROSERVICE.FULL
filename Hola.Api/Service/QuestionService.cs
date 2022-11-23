using Hola.Api.Models;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using static Google.Cloud.Storage.V1.UrlSigner;
using static Hola.Core.Common.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hola.Core.Common;
using Microsoft.VisualBasic;

namespace Hola.Api.Service
{
    public class QuestionService : BaseService
    {


        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.DEFAULT_DB;
        public QuestionService(IOptions<SettingModel> options) : base(options)
        {
            _options = options;
        }

        public async Task<List<CategoryModel>> GetAllQuestion()
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var sql = "SELECT id, name, define, created_on FROM qes.categories;";
            var result = await QueryToListAsync<CategoryModel>(setting.Connection, sql);

            return result;
        }
    }
}
