using Hola.Api.Models.Categories;
using Hola.Api.Models.Questions;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class CategoryService : BaseService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.DEFAULT_DB;
        private string ConnectionString = string.Empty;
        public CategoryService(IOptions<SettingModel> options) : base(options)
        {
            _options = options;
            ConnectionString = _options.Value.Connection + "Database=" + database;
        }

        public async Task<bool> AddCategory(AddCategoryModel addCategory, int userid)
        {
            var sql = string.Format("INSERT INTO usr.categories(name, define, created_on, \"Image\",fk_userid,totalquestion,priority) " +
                "VALUES ( '{0}', '{1}', now(), '{2}',{3},0,0);",
                addCategory.Name, addCategory.Define, addCategory.Image, userid);
            var result = await Excecute(ConnectionString, sql);
            return true;
        }

    }
}
