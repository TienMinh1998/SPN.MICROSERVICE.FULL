using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Requests;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.ExcelServices;
using Hola.Api.Service.ExcelServices.DataConfig;
using Hola.Api.Service.ExcelServices.Enum;
using Hola.Api.Service.ExcelServices.TestModels;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Utils;
using iText.IO.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Quartz.Util;
using SPNApplication.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static Hola.Api.Service.ExcelServices.ExcelService;

namespace Hola.Api.Controllers
{
    [Route("QuestionStandard")]
    public class QuestionStandardController : ControllerBase
    {
        private IQuestionStandardService _questionStandardService;
        private readonly IMapper _mapper;
        private readonly DapperBaseService _dapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public QuestionStandardController(IQuestionStandardService questionStandardService,
            IMapper mapper,
            DapperBaseService dapper,
            IWebHostEnvironment hostEnvironment)
        {
            _questionStandardService = questionStandardService;
            _mapper = mapper;
            _dapper = dapper;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Lấy về câu hỏi theo ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionById/{Id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetQuestionById(int Id)
        {
            try
            {
                var response = await _questionStandardService.GetFirstOrDefaultAsync(x => x.Pk_QuestionStandard_Id == Id);
                if (response != null)
                {
                    return JsonResponseModel.Success(response, $"Lấy về từ có Id = {Id} thành công!");
                }
                else
                {
                    return JsonResponseModel.Error("Có lỗi trong quá trình lấy câu hỏi", 400);
                }
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpGet("admin_infomation")]
        public JsonResponseModel GetInfomation()
        {
            try
            {
                string sql = "SELECT count(1) from public.\"QuestionStandards\";";
                int? _count = _dapper.QueryFirstOrDefault<int>(sql);

                if (_count != null)
                {
                    return JsonResponseModel.Success(_count.Value, $"SUCCESS");
                }
                else
                {
                    return JsonResponseModel.Error("Có lỗi trong quá trình lấy câu hỏi", 400);
                }
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpGet("admin_search/{word}")]
        public async Task<JsonResponseModel> Search(string word)
        {
            try
            {
                //  Lấy ra audio của từ đó, nếu không lấy được audio thì mặc định để trống;
                // Nếu từ đó có dấu cách thì thôi không lấy audio nữa 
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string camAudio = string.Empty;
                string camPhonetic = string.Empty;
                string note = string.Empty;
                string camType = string.Empty;

                bool isBasicWord = word.Trim().ToLower().Contains(" ");

                if (!isBasicWord)
                {
                    try
                    {
                        APICrossHelper api = new APICrossHelper();
                        Task<CambridgeDictionaryModel> cambridgeDicTask = api.GetWord(word);
                        Task<CambridgeDictionaryVietNamModel> vietnamMeaningTask = api.GetVietNamMeaning(word);

                        await Task.WhenAll(cambridgeDicTask, vietnamMeaningTask);
                        var cambridgeDicResponse = cambridgeDicTask.Result;
                        var vietNamMeaningResponse = vietnamMeaningTask.Result;
                        camAudio = cambridgeDicResponse?.Mp3;
                        camPhonetic = cambridgeDicResponse?.Phonetic;
                        note = $",{string.Join(',', vietNamMeaningResponse.Meaning)} ";
                        camType = cambridgeDicResponse.Type;
                    }
                    catch (Exception)
                    {

                    }
                }
                SearchVocabularyModel searchResult = new SearchVocabularyModel();
                searchResult.phonetic = camPhonetic;
                searchResult.audio = camAudio;
                searchResult.Type = camType;
                searchResult.searchText = word;



                string typeNote = camType.Trim().ToLower();
                switch (typeNote)
                {
                    case "adverb":
                        typeNote = "(adv)";
                        break;
                    case "adjective":
                        typeNote = "(adj)";
                        break;
                    case "noun":
                        typeNote = "(N)";
                        break;
                    case "verb":
                        typeNote = "(V)";
                        break;
                }

                return JsonResponseModel.Success(searchResult);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Lấy ra tất cả các từ, column nhập vào tên trường muốn sắp xếp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AllQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> Search([FromBody] QuestionModelStandard request)
        {
            try
            {
                // lấy ra câu hỏi của người dùng đó
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                bool condition = false;
                Func<QuestionStandard, bool> searchCondition = x => ((!string.IsNullOrEmpty(request.searchKey)) ? x.English.Contains(request.searchKey) : true) && x.UserId == userid;

                if (request.IsDesc == null || request.IsDesc == false)
                {
                    condition = false;
                }
                else
                {
                    condition = true;
                }
                var question = _questionStandardService.GetListPaged(request.PageNumber, request.PageSize, searchCondition, request.columnname, condition);
                question.currentPage = request.PageNumber;
                return JsonResponseModel.Success(question);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Thêm một từ mới 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddStandardQuestion")]
        public async Task<JsonResponseModel> Add([FromBody] AddQuestionStandardModel request)
        {
            try
            {
                //  Lấy ra audio của từ đó, nếu không lấy được audio thì mặc định để trống;
                // Nếu từ đó có dấu cách thì thôi không lấy audio nữa 
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string camAudio = string.Empty;
                string camPhonetic = string.Empty;
                string note = string.Empty;
                string camType = string.Empty;

                bool isBasicWord = request.English.Trim().ToLower().Contains(" ");

                if (!isBasicWord)
                {
                    try
                    {
                        string word = request.English;
                        APICrossHelper api = new APICrossHelper();
                        Task<CambridgeDictionaryModel> cambridgeDicTask = api.GetWord(word);
                        Task<CambridgeDictionaryVietNamModel> vietnamMeaningTask = api.GetVietNamMeaning(word);

                        await Task.WhenAll(cambridgeDicTask, vietnamMeaningTask);
                        var cambridgeDicResponse = cambridgeDicTask.Result;
                        var vietNamMeaningResponse = vietnamMeaningTask.Result;
                        camAudio = cambridgeDicResponse?.Mp3;
                        camPhonetic = cambridgeDicResponse?.Phonetic;
                        note = $",{string.Join(',', vietNamMeaningResponse.Meaning)} ";
                        camType = cambridgeDicResponse.Type;
                    }
                    catch (Exception)
                    {

                    }
                }

                string typeNote = camType.Trim().ToLower();
                switch (typeNote)
                {
                    case "adverb":
                        typeNote = "(adv)";
                        break;
                    case "adjective":
                        typeNote = "(adj)";
                        break;
                    case "noun":
                        typeNote = "(N)";
                        break;
                    case "verb":
                        typeNote = "(V)";
                        break;
                }

                var command = _mapper.Map<QuestionStandard>(request);
                command.created_on = DateTime.UtcNow;
                command.IsDeleted = false;
                command.Audio = camAudio;
                command.UserId = userid;
                command.Phonetic = camPhonetic;
                command.Note = $"{typeNote} " + note;


                var checkquestion = await _questionStandardService.GetFirstOrDefaultAsync(x => x.English == request.English && x.UserId == userid);
                if (checkquestion != null)
                {
                    return JsonResponseModel.SERVER_ERROR($"{request.English} is available");
                }
                var respoinse = await _questionStandardService.AddAsync(command);
                return JsonResponseModel.Success(respoinse);


            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Lấy ra các từ theo chủ đề
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Get_StandQuesByTopic")]
        [Authorize]
        public async Task<JsonResponseModel> GetAllQuestionByTopic([FromBody] GetStandQuestionRequest request)
        {
            try
            {
                string query = "SELECT a.\"Pk_QuestionStandard_Id\",  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\",  a.\"MeaningVietNam\"   FROM  (public.\"QuestionStandards\" q " +
                    "\r\ninner join usr.\"QuestionStandardDetail\" qd on q.\"Pk_QuestionStandard_Id\"" +
                    $" = qd.\"QuestionID\" ) a\r\ninner join usr.topic tq on tq.\"PK_Topic_Id\" = a.\"TopicID\"\r\nwhere a.\"TopicID\" = {request.TargetID}";

                var response = await _dapper.GetAllAsync<QuestionStandardModel>(query.AddPadding(request.pageNumber, request.PageSize));
                return JsonResponseModel.Success(response);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpPost("Get_StandQuesByTopic/app")]
        [Authorize]
        public async Task<JsonResponseModel> GetAllQuestionByTopic_app([FromBody] GetStandQuestionRequest request)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string query = "SELECT a.\"Pk_QuestionStandard_Id\",  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\", " +
                    " a.\"MeaningVietNam\", \r\ncase WHEN usq.\"StandardQuestion\" > 0 then true else false END as \"Tick\"\r\nFROM " +
                    " ((public.\"QuestionStandards\" q \r\ninner join usr.\"QuestionStandardDetail\" qd on q.\"Pk_QuestionStandard_Id\" " +
                    "= qd.\"QuestionID\" ) a inner join usr.topic tq on tq.\"PK_Topic_Id\" = a.\"TopicID\") \r\nleft join usr.\"UserStandardQuestion\" " +
                    $"usq on (a.\"Pk_QuestionStandard_Id\"  = usq.\"StandardQuestion\" and usq.\"UserId\" ={userid}) where a.\"TopicID\" = {request.TargetID} order by a.\"created_on\" asc";

                var response = await _dapper.GetAllAsync<QuestionStandardModel>(query.AddPadding(request.pageNumber, request.PageSize));
                return JsonResponseModel.Success(response);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Thêm từ vào topic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("AddQuestionToTopic")]
        [Authorize]
        public async Task<JsonResponseModel> AddQuestionToTopic([FromBody] AddQuestionToTopic1 model)
        {
            try
            {
                // Thêm câu hỏi vào topic
                if (string.IsNullOrEmpty(model.QuestionID.ToString()) || string.IsNullOrEmpty(model.TopicID.ToString()))
                    return JsonResponseModel.Success("Sai định dạng dữ liệu đầu vào!");
                string sql_Add = $"INSERT INTO usr.\"QuestionStandardDetail\" (\"QuestionID\", \"TopicID\") VALUES({model.QuestionID}, {model.TopicID});";
                var response = _dapper.Execute(sql_Add);
                // Cập nhật lại trường đã thêm vào topic bằng true 
                string sqlUpdate = $"UPDATE public.\"QuestionStandards\" SET \"Added\"=true WHERE \"Pk_QuestionStandard_Id\"={model.QuestionID}";
                var responseUpdatge = _dapper.Execute(sqlUpdate);

                return JsonResponseModel.Success("Thêm thành công");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Cập nhật câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("UpdateQuestionStandard")]
        [Authorize]
        public async Task<JsonResponseModel> Update([FromBody] UpdateQuestionStandardModel model)
        {
            try
            {

                string count = $"SELECT COUNT(1) FROM public.\"QuestionStandards\" where \"Pk_QuestionStandard_Id\" = {model.Id}";
                var count_Response = _dapper.QueryFirstOrDefault<int>(count);
                if (count_Response != 1)
                    return JsonResponseModel.Error("Câu hỏi không tồn tại, vui lòng thử lại", 400);
                string sql_update = $"UPDATE public.\"QuestionStandards\"\r\nSET \"English\"='{model.English}', \"Phonetic\"='{model.Phonetic}', \"MeaningEnglish\"='{model.MeaningEnglish}'," +
                    $" \"MeaningVietNam\"='{model.MeaningVietNam}', \"Note\"='{model.Note}'\r\nWHERE \"Pk_QuestionStandard_Id\"={model.Id};";
                var response = _dapper.Execute(sql_update);
                return JsonResponseModel.Success("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("QuestionStandard/{id}")]
        public async Task<JsonResponseModel> DeleteQuestion(int id)
        {
            try
            {
                var question = await _questionStandardService.GetFirstOrDefaultAsync(x => x.Pk_QuestionStandard_Id == id);
                if (question == null)
                    return JsonResponseModel.Error($"Từ mới Id='{id}' không tồn tại", 400);
                question.IsDeleted = true;
                await _questionStandardService.UpdateAsync(question);
                return JsonResponseModel.Success(new List<string>(), $"Xóa thành công từ mới Id ='{id}'");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, 500);
            }

        }


        [HttpGet("History")]
        [Authorize]
        public async Task<JsonResponseModel> GetHistory()
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string query = "SELECT id, fk_userid, count_word, percent_day, note, created_on " +
                    $"FROM history.userhistory where fk_userid ={userid} order by created_on desc ";
                var response = await _dapper.GetAllAsync<HistoryModel>(query);
                return JsonResponseModel.Success(response);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpPost("Tick")]
        [Authorize]
        public async Task<JsonResponseModel> TickQuestion([FromBody] TickQuestionRequest request)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string sql = $"INSERT INTO usr.\"UserStandardQuestion\" (\"StandardQuestion\", \"UserId\") VALUES({request.QuestionStandardId}, {userid});";
                await _dapper.Execute(sql);
                return JsonResponseModel.Success("Update thành công");
            }
            catch (Exception)
            {
                return JsonResponseModel.Error("Thêm thất bại. Từ đã tồn tại", 400);

            }
        }


        [HttpPost("ExportWordForTopic")]
        [ApiVersion("3")]
        public async Task<IActionResult> ExportExcel([FromBody] ExcelModelRequest request)
        {
            try
            {
                var queryGetId = "SELECT \"PK_Topic_Id\" FROM usr.topic where lower(Trim(\"EnglishContent\"))=lower(Trim('water shortage'));";
                var topicId = _dapper.QueryFirstOrDefault<int>(queryGetId);

                if (topicId == 0) return null;
                string query = "SELECT a.\"Pk_QuestionStandard_Id\",  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\",  a.\"MeaningVietNam\", a.\"Note\" " +
                    "  FROM  (public.\"QuestionStandards\" q " +
                   "\r\ninner join usr.\"QuestionStandardDetail\" qd on q.\"Pk_QuestionStandard_Id\"" +
                   $" = qd.\"QuestionID\" ) a\r\ninner join usr.topic tq on tq.\"PK_Topic_Id\" = a.\"TopicID\"\r\nwhere a.\"TopicID\" = {topicId}  order by a.\"Pk_QuestionStandard_Id\"";

                var response = await _dapper.GetAllAsync<QuestionStandardModel>(query.AddPadding(1, 30));


                // Report 2 : 
                string query2 = "SELECT \"Id\", fk_userid, \"name\", define, \"Image\", totalquestion, priority, created_on FROM usr.categories;";
                var response2 = await _dapper.GetAllAsync<CategoryModel>(query2);


                ReportConfiguration option = new ReportConfiguration();
                option.ExHeader = new List<HeaderOfPager>()
                    {
                        new HeaderOfPager()
                        {
                            StartRow = 1,
                            StartColumn = 1,
                            Size = 10,
                            Align = "center",
                            ColumnSpan = 3,
                            TopContent = "SỞ GIAO THÔNG VẬN TẢI THÁI BÌNH",
                            BottomContent = "Trung tâm Bình Anh",
                            FontFamily = "Times New Roman",
                            FontWeigh = "",
                            Visible = true
                        },
                        new HeaderOfPager()
                        {
                            StartRow = 1,
                            StartColumn = 4,
                            ColumnSpan = 4,
                            Size = 10,
                            Align = "center",
                            TopContent = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM",
                            BottomContent = "Độc lập - Tự do - Hạnh Phúc",
                            FontFamily = "Times New Roman",
                            FontWeigh = "",
                            Visible = true
                        },
                    };
                option.Body = new Body()
                {
                    Datagriviews = new List<ExcelDatagriview>()
                  {
                     new ExcelDatagriview
                     {
                         Type = DATAGRIVIEW_TYPE.TEXT,
                         MarginTop = 1,
                         Content = "BÁO CÁO QUÁ TRÌNH ĐÀO TẠO THỰC HÀNH LÁI XE CỦA HỌC VIÊN",
                         SubContent = "(Ngày báo cáo: 11/01/2023)",
                         Size= 16,
                         SizeSub = 10,
                         FontWeigh = "B",
                         Align= "center",
                         FontFamily = "Times new roman",
                         StartColumnOfContent = 1,
                         Images = new List<ImageProperties>
                         {
                               new ImageProperties
                                {
                                    Name = "image1",
                                    ColumnIndex= 6,
                                    RowIndex= 11,
                                    Height = 100,
                                    Width= 100,
                                    ImageUrl ="https://1.vikiplatform.com/pr/15639pr/3611116f24.jpg?x=b&s=460x268&e=t&f=t&cb=1"
                                },
                               new ImageProperties
                                {
                                    Name = "image2",
                                    ColumnIndex= 6,
                                    RowIndex= 18,
                                    Height = 100,
                                    Width= 100,
                                    ImageUrl ="https://lh3.googleusercontent.com/TJavz_7TNPwqc2oa1HQDKcfKkXF38it5W9ph_aMHuXwt9gKRtZQiwdKG8YD_hgO9x70_v1k1yMtihuRi4It0tN2vLvpHJy9_VQ=w960-rj-nu-e365"
                                }
                         },
                         InfomationText = new List<string>()
                         {
                             "1. HỌ VÀ TÊN : NGUYỄN VIẾT MINH TIẾN",
                             "2. MÃ HỌC VIÊN : 123456789",
                             "3. NGÀY SINH : 05.02.1999",
                             "4. HẠNG ĐÀO TẠO : B2",
                             "5. CƠ SỞ ĐÀO TẠO : TRUNG TÂM GIÁO DỤC NGHỀ NGHIỆP VÀ SÁT HẠCH LÁI XE HÀ NỘI"
                         },
                         MergeColumnTitle = 7,
                      },
                     new ExcelDatagriview
                     {
                         Type = DATAGRIVIEW_TYPE.TABLE,
                         MarginTop = 2,
                         Content = "I, BÁO CÁO THỰC SỐ LƯỢNG HỌC VIÊN HOÀN THÀNH NỘI DUNG HỌC THỰC HÀNH LÁI XE TRÊN ĐƯỜNG GIAO THÔNG CÔNG CỘNG CỦA CƠ SỞ ĐÀO TẠO",
                         SubContent = "Từ ngày :12/09/2022 đến ngày : 29/12/2022",
                         Size= 16,
                         SizeSub = 10,
                         FontWeigh = "B",
                         Align= "center",
                         FontFamily = "Times new roman",
                         StartColumnOfContent = 1,
                         Data = response.ToList(),
                         Headers = new List<HeaderOfTable>
                         {
                             new HeaderOfTable {
                                 ColumnName ="English",
                                 DisplayName = "Từ vựng",
                                 Width = 12,
                                 Location = 2,
                                 TextAlign = "center"
                             },
                             new HeaderOfTable { ColumnName = "Phonetic", DisplayName = "Phiên âm",Width = 12, Location = 1},
                             new HeaderOfTable { ColumnName = "MeaningEnglish", DisplayName = "Tiếng Anh",Width = 12, Location = 3},
                             new HeaderOfTable { ColumnName = "MeaningVietNam", DisplayName = "Tiếng Việt",Width = 12, Location = 4},
                             new HeaderOfTable { ColumnName = "Note", DisplayName = "Ghi chú",Width = 12, Location = 5},
                             new HeaderOfTable { ColumnName = "Pk_QuestionStandard_Id", DisplayName = "ID",Width = 12, Location = 6},
                             new HeaderOfTable { ColumnName = "Tick", DisplayName = "Trạng Thái",Width = 12, Location = 7},
                         },
                         Footers = new List<FootterConfig>
                         {
                             new FootterConfig
                             {
                                 Content = "Tổng Cộng",
                                 MergeRow = 1,
                                 StartColumn = 1
                             },
                              new FootterConfig
                             {
                                 Content = "1,200,000 VNĐ",
                                 MergeRow = 5,
                                 StartColumn = 2
                             },
                               new FootterConfig
                             {
                                 Content = "",
                                 MergeRow = 1,
                                 StartColumn = 7
                             },
                         }
                     },
                     new ExcelDatagriview
                     {
                         Type = DATAGRIVIEW_TYPE.TABLE,
                         MarginTop= 2,
                         Content = "II. BÁO CÁO KẾT QUẢ ĐÀO TẠO THỰC HÀNH LÁI XE TRÊN ĐƯỜNG GIAO THÔNG THEO DANH SÁCH THÍ SINH DỰ SÁT HẠCH",
                         SubContent = "(Mã báo cáo 2 : BC2 66015_66_66015K22B2001)",
                         Size= 16,
                         SizeSub =10,
                         BacgroundColor = "#ffffff",
                         TextHeaderColor = "#White",
                         FontWeigh = "B",
                         Align= "center",
                         FontFamily = "Times new roman",
                         StartColumnOfContent = 1,
                         Data = response2.ToList(),
                         Headers = new List<HeaderOfTable>
                         {
                             new HeaderOfTable
                             {
                                 ColumnName ="Id",
                                 DisplayName = "Từ vựng",
                                 Width = 10,
                                 Location = 1,
                                 ColumnSpan = 2
                             },
                              new HeaderOfTable
                             {
                                 ColumnName = "EmtyColumn",
                                 DisplayName = "Phiên âm",
                                 Width =25 ,
                                 Location = 2
                             },
                             new HeaderOfTable
                             {
                                 ColumnName = "Name",
                                 DisplayName = "Phiên âm",
                                 Width =25 ,
                                 Location = 3
                             },
                             new HeaderOfTable
                             {
                                 ColumnName = "define",
                                 DisplayName = "Tiếng Anh",
                                 Width = 30,
                                 Location = 4,
                                 ColumnSpan = 2
                             },
                             new HeaderOfTable
                             {
                                 ColumnName = "emptyDefine",
                                 DisplayName = "Tiếng Việt",
                                 Width = 20,
                                 Location = 5
                             },
                             new HeaderOfTable
                             {
                                 ColumnName = "Image",
                                 DisplayName = "Tiếng Việt",
                                 Width = 20,
                                 Location = 6
                             },
                             new HeaderOfTable
                             {
                                 ColumnName = "TotalQuestion",
                                 DisplayName = "Ghi chú",
                                 Width = 20,
                                 Location = 7
                             },
                         },
                         Footers = new List<FootterConfig>
                         {
                             new FootterConfig
                             {
                                 Content = "NGUYỄN VIẾT MINH TIẾN",
                                 MergeRow = 1,
                                 StartColumn = 1
                             },
                             new FootterConfig
                             {
                                 Content = "1,200,000 VNĐ",
                                 MergeRow = 4,
                                 StartColumn = 2
                             },
                             new FootterConfig
                             {
                                 Content = "Đạt",
                                 MergeRow = 2,
                                 StartColumn = 6
                             },
                         }
                     }
                  }
                };


                ExcelService extension = new ExcelService(option);

                var fileUrl = await extension.ExportExcel<QuestionStandardModel, CategoryModel, CategoryModel>();
                var filename = Path.GetFileName(fileUrl);
                var file = await Task.FromResult(File(System.IO.File.ReadAllBytes(fileUrl), "application/octet-stream", filename));
                file.FileDownloadName = $"{DateTime.Now.ToString()}.xlsx";
                FileInfo fileRemove = new FileInfo(fileUrl);
                fileRemove.Delete();
                return file;
            }
            catch (Exception ex)
            {
                return default(ActionResult);
            }
        }

        /// <summary>
        /// Quá trình đào tạo thực hành lái xe của học viên
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Bao_cao_1")]
        [ApiVersion("3")]
        public async Task<IActionResult> ExportExcel1([FromBody] ReportConfiguration request)
        {
            try
            {
                var response = new List<Student>();
                for (int i = 0; i < 40; i++)
                {
                    response.Add(new Student
                    {
                        LoginTime = "Test",
                        LogoutTime = "Test",
                        Session = "Test",
                        STT = i,
                        Time = "Test",
                        url = "https://pbs.twimg.com/media/BfrLniWCAAEHuD5.jpg"
                    });
                }

                ReportConfiguration option = new ReportConfiguration();
                // left header 
                var leftHeader = (new HeaderBuilder()).SetStartRow(1)
                    .SetStartColumn(1)
                    .SetSize(10)
                    .SetAlign("center")
                    .SetColumnSpan(3)
                    .SetTopContent("SỞ GIAO THÔNG VẬN TẢI THÁI BÌNH")
                    .SetBottomContent("Trung tâm Bình Anh")
                    .SetDecorate().build;

                // right header 
                var rightHeader = (new HeaderBuilder())
                    .SetStartRow(1)
                    .SetStartColumn(5)
                    .SetSize(10)
                    .SetAlign("center")
                    .SetColumnSpan(5)
                    .SetTopContent("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                    .SetBottomContent("Độc lập - Tự do - Hạnh phúc")
                    .SetDecorate().build;

                // dataContent
                var topContent = (new ExcelDatagriviewBuilder())
                .SetTitle("BÁO CÁO QUÁ TRÌNH ĐÀO TẠO THỰC HÀNH LÁI XE CỦA HỌC VIÊN", 8)
                .SetType(DATAGRIVIEW_TYPE.TEXT)
                .SetDecorate()
                .SetSubTitle("(Ngày báo cáo: 11/01/2023)")
                .SetMarginTop(2)
                .SetImage(new List<ImageProperties>
                         {
                                new ImageProperties
                                {
                                    Name = "image1",
                                    ColumnIndex= 6,
                                    RowIndex= 7,
                                    Height = 115,
                                    Width= 120,
                                    ImageUrl ="https://1.vikiplatform.com/pr/15639pr/3611116f24.jpg?x=b&s=460x268&e=t&f=t&cb=1",
                                    MarginLeft= 20,
                                    MarginTop= 5,
                                }
                         })
                .SetInfomation(new List<string>()
                         {
                             "1. HỌ VÀ TÊN : NGUYỄN VIẾT MINH TIẾN",
                             "2. MÃ HỌC VIÊN : 123456789",
                             "3. NGÀY SINH : 05.02.1999",
                             "4. HẠNG ĐÀO TẠO : B2",
                             "5. CƠ SỞ ĐÀO TẠO : TRUNG TÂM GIÁO DỤC NGHỀ NGHIỆP VÀ SÁT HẠCH LÁI XE HÀ NỘI"
                         }).Build;
                var bodyContent = (new ExcelDatagriviewBuilder())
               .SetTitle("BÁO CÁO QUÁ TRÌNH ĐÀO TẠO THỰC HÀNH LÁI XE CỦA HỌC VIÊN 1", 9)
               .SetType(DATAGRIVIEW_TYPE.TABLE)
               .SetDecorate()
               .SetSubTitle("(Ngày báo cáo: 11/01/2023)")
               .SetMarginTop(2)
               .SetCollection(response.ToList())
               .SetHeaderConfiguration(new List<HeaderOfTable>
                         {
                             new HeaderOfTable {
                                 ColumnName = "English",
                                 DisplayName = "Từ vựng",
                                 Width = 12,
                                 Location = 1,
                                 TextAlign = "center"
                             },
                             new HeaderOfTable { ColumnName = "STT", DisplayName = "Phiên âm", Width = 12, Location = 1,ColumnSpan = 2 },
                             new HeaderOfTable {  ColumnName = "EmptyColumn", DisplayName = "Phiên âm", Location = 2 },
                             new HeaderOfTable { ColumnName = "Session", DisplayName = "Tiếng Anh", Width = 12, Location = 3 },
                             new HeaderOfTable { ColumnName = "LoginTime", DisplayName = "Tiếng Việt", Width = 12, Location = 4 },
                             new HeaderOfTable { ColumnName = "LogoutTime", DisplayName = "Ghi chú", Width = 12, Location = 5 },
                             new HeaderOfTable { ColumnName = "Time", DisplayName = "ID", Width = 12, Location = 6 },
                             new HeaderOfTable { ColumnName = "url", DisplayName = "Trạng Thái", Width = 12, Location = 7,ColumnSpan= 2},
                             new HeaderOfTable { ColumnName = "url", DisplayName = "Trạng Thái", Width = 12, Location = 7 }
                         })
               .SetFooter(new List<FootterConfig>
               {
                   new FootterConfig
                   {
                       Content = "Tổng Cộng",
                       MarginTop= 0,
                       MergeRow = 1,
                       StartColumn =1,
                   },
                   new FootterConfig
                   {
                       Content = "1230",
                       MarginTop= 0,
                       MergeRow = 5,
                       StartColumn = 2,
                   },
                     new FootterConfig
                   {
                       Content = "Đạt tiêu chuẩn",
                       MarginTop= 0,
                       StartColumn = 7,
                       MergeRow = 2,

                   },
               }).Build;

                option.ExHeader.Add(leftHeader);
                option.ExHeader.Add(rightHeader);
                option.Body.Datagriviews.Add(topContent);
                option.Body.Datagriviews.Add(bodyContent);
                TextPlus text = new TextPlus()
                {
                    Align = "right",
                    Col = 1,
                    Row = 5,
                    ColSpan = 8,
                    Content = "Hà nội, ngày 27 tháng 1 năm 2023",
                    FontFamily = "Times new roman",
                    FontWeigh = "I",
                    Size = 10
                };
                option.Texts.Add(text);

                ExcelService extension = new ExcelService(option);
                var fileUrl = await extension.ExportExcel<Student, CategoryModel, CategoryModel>();

                var filename = Path.GetFileName(fileUrl);
                var file = await Task.FromResult(File(System.IO.File.ReadAllBytes(fileUrl), "application/octet-stream", filename));
                file.FileDownloadName = $"{DateTime.Now.ToString()}.xlsx";
                FileInfo fileRemove = new FileInfo(fileUrl);
                fileRemove.Delete();
                return file;
            }
            catch (Exception ex)
            {
                return default(ActionResult);
            }
        }

        // Thông kê
        [HttpPost("overview")]
        [Authorize]
        public async Task<JsonResponseModel> GetOverview([FromBody] OverviewRequest request)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var startDate = request.StartTime?.ToString("yyyy/MM/dd");
                var endDate = request.EndTime?.ToString("yyyy/MM/dd");
                string query = string.Empty;
                if (request.StartTime.HasValue && request.EndTime.HasValue)
                {
                    query = $"SELECT \"Id\", \"FK_UserId\", \"TotalWords\", \"TotalPosts\", created_on\r\nFROM usr.report where created_on <= '{startDate}' and created_on >= '{endDate}'";
                }
                else
                {
                    // var listResult = await _reportService.GetAllAsync(x => x.FK_UserId == userid);
                    query = $"SELECT \"Id\", \"FK_UserId\", \"TotalWords\", \"TotalPosts\", created_on FROM usr.report where \"FK_UserId\"={userid} ORDER by  created_on  Asc limit 30";
                }
                var response = await _dapper.GetAllAsync<Report>(query);
                return JsonResponseModel.Success(response, "Thành công");
            }
            catch (Exception)
            {
                return JsonResponseModel.Error("Thêm thất bại. Từ đã tồn tại", 400);

            }
        }

    }
}

