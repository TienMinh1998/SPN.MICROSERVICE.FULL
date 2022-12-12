﻿using System;
using Hola.Api.Models;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Threading.Tasks;
using Hola.Core.Common;
using Hola.Api.Models.Questions;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EntitiesCommon.EntitiesModel;
using System.Linq;

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

        public async Task<List<CategoryModel>> GetAllCategory(int userid)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var sql = string.Format("SELECT id, name, define, created_on, \"Image\", totalquestion FROM qes.categories WHERE fk_userid={0} order by created_on DESC", userid);
            var result = await QueryToListAsync<CategoryModel>(setting.Connection, sql);

            return result;
        }

        public async Task<List<QuestionModel>> GetListQuestionByCategoryId(int categoryID, int is_Delete)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var sql = $"SELECT id, category_id, questionname, answer, created_on, is_delete,\"ImageSource\", audio FROM qes.question WHERE category_id= {categoryID} and is_delete = {is_Delete} ORDER BY created_on DESC;";
            var result = await QueryToListAsync<QuestionModel>(setting.Connection, sql);
            return result;
        }

        public async Task<bool> AddQuestion(QuestionAddModel addQuestion)
        {

            // phiên âm của từ đó
            APICrossHelper api = new APICrossHelper();
            string word = addQuestion.QuestionName;
            var response =await api.Get<object>($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
            var phienam = JsonConvert.DeserializeObject<List<ResponseDicModel>>(response.ToString());
            string phonetic = phienam.FirstOrDefault().phonetic;
            var  audio = phienam.FirstOrDefault().phonetics.FirstOrDefault().audio;

            addQuestion.QuestionName += $" {phonetic}";
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            // Add new Question
            var sql = "insert into qes.question (category_id, questionname, answer, created_on,\"ImageSource\", fk_userid, phonetic, audio) " +
                      $"values ({addQuestion.Category_Id},'{addQuestion.QuestionName}','{addQuestion.Answer}',now(),'{addQuestion.ImageSource}',{addQuestion.fk_userid},'{phonetic}','{audio}');";

            var countQuery = $"SELECT COUNT(1) FROM qes.question WHERE category_id={addQuestion.Category_Id}";
            var countResponse = await ExcecuteScalarAsync(setting.Connection, countQuery);
            var updateCategoryQuery = string.Format("UPDATE qes.categories SET totalquestion = {0} WHERE id = {1};", countResponse + 1, addQuestion.Category_Id);
            var responseupdate = await Excecute(setting.Connection, updateCategoryQuery);
            var result = await Excecute(setting.Connection, sql);
            return true;
        }

        public async Task<bool> DeleteQuestion(int questionID)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sql = $"UPDATE qes.question SET is_delete = 1 WHERE id = {questionID};";
            var result = await Excecute(setting.Connection, sql);

            return true;
        }

        public async Task<int> CountQuestion()
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sql = $"SELECT COUNT(1) FROM qes.question;";
            var result = await ExcecuteScalarAsync(setting.Connection, sql);
            return result;
        }

        public async Task<int> CountQuestionToday(int userid)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sql = $"SELECT COUNT(1) FROM qes.question where fk_userid = {userid} and created_on::Date = CURRENT_DATE";
            var result = await ExcecuteScalarAsync(setting.Connection, sql);
            return result;
        }
    }
}
