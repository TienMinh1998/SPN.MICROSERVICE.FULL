using System;
using Hola.Api.Models;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Threading.Tasks;
using Hola.Core.Common;
using Hola.Api.Models.Questions;

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

        public async Task<List<CategoryModel>> GetAllCategory()
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

        public async Task<List<QuestionModel>> GetListQuestionByCategoryId(int categoryID)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var sql = $"SELECT id, category_id, questionname, answer, created_on, is_delete FROM qes.question WHERE category_id= {categoryID}";
            var result = await QueryToListAsync<QuestionModel>(setting.Connection, sql);
            return result;
        }

        public async Task<bool> AddQuestion(QuestionAddModel addQuestion)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var sql = "insert into qes.question (category_id, questionname, answer, created_on) " +
                      $"values ({addQuestion.Category_Id},'{addQuestion.QuestionName}','{addQuestion.Answer}','{DateTime.Now}');";
            var result = await Excecute(setting.Connection, sql);

            return true;
        }
    }
}
