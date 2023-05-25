using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Requests.Phrase;
using Hola.Api.Requests.Reading;
using Hola.Api.Service;
using Hola.Api.Service.BAImportExcel;
using Hola.Core.DapperExtension;
using Hola.Core.Model;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [Route("phrase")]
    [ApiController]
    [Authorize]
    public class PhraseController : ControllerBase
    {
        private readonly IPhraseService _phraseService;
        private readonly IReadingService _readingService;
        public PhraseController(IPhraseService phraseService, IReadingService readingService)
        {
            _phraseService = phraseService;
            _readingService = readingService;
        }

        /// <summary>
        /// Thêm mới từ 1 file excel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("import/excel")]
        public async Task<JsonResponseModel> Import([FromForm] ImportExcelPhraseRequest request)
        {
            try
            {
                Dictionary<string, string> message = new Dictionary<string, string>();
                ExcelLibImport service = new ExcelLibImport();
                var headerNames = new List<string>()
            {
                "STT","Word","Definition"
            };
                var input = (new ExcelImportRequestBuilder())
                                  .SetStartRow(2)
                                  .SetWorkSheetIndex(1)
                                  .SetPaddingBottom(0)
               .SetFile(request.file)
                                  .SetHeaderColumn(headerNames)
                                  .Build;
                var response = service.ConvertToList<PhraseModel>(input);
                foreach (var item in response)
                {
                    string word = item.Word.Trim();
                    var phrase = await _phraseService.GetFirstOrDefaultAsync(x => x.word == item.Word && x.fk_readingId == request.ReadingId && x.IsDeleted != 1);
                    if (phrase == null)
                    {
                        var entity = new Phrase
                        {
                            CreatedDate = DateTime.UtcNow,
                            definition = item.Definition,
                            IsDeleted = 0,
                            fk_readingId = request.ReadingId,
                            word = item.Word
                        };
                        var result = await _phraseService.AddAsync(entity);
                        message.Add($"{item.Word}", "Thành công");
                    }
                    else
                    {
                        phrase.word = word;
                        phrase.definition = item.Definition;
                        _ = _phraseService.UpdateAsync(phrase);
                        message.Add($"{item.Word}", "Đã cập nhật lại");
                    }
                }
                return JsonResponseModel.Success(message);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
        }

        /// <summary>
        /// Lấy ra chi tiết của phrase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var phrase = await _phraseService.GetFirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != 1);
                if (phrase != null)
                {
                    return StatusCode(200, phrase);
                }
                else
                {
                    return StatusCode(200, null);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error, {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm một cụm từ liên quan vào bài đọc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<JsonResponseModel> Add([FromBody] AddPhraseRequest model)
        {
            try
            {
                var reading = await _readingService.GetFirstOrDefaultAsync(x => x.Id == model.ReadingId && x.IsDeleted != 1);
                if (reading == null)
                {
                    return JsonResponseModel.Error($"this reading which have id = '{model.ReadingId}' is not available or deleted by anyone", 400);
                }
                // CHeck the phrase have availavle? 
                var old_phrase = await _phraseService.GetFirstOrDefaultAsync(x => x.word == model.word && x.fk_readingId == model.ReadingId);
                if (old_phrase != null)
                {
                    return JsonResponseModel.Error($"this phrase is available, check again", 400);
                }

                Phrase entity = new Phrase()
                {
                    word = model.word,
                    definition = model.Meaning,
                    fk_readingId = model.ReadingId
                };
                var response = await _phraseService.AddAsync(entity);
                if (response != null)
                {
                    return JsonResponseModel.Success(response);
                }
                else
                {
                    return JsonResponseModel.Error("can't add to phrase", 400);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Sửa 1 phrase trong reading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        public async Task<JsonResponseModel> Edit([FromBody] UpdatePhraseRequest model)
        {
            try
            {
                var phrase = await _phraseService.GetFirstOrDefaultAsync(x => x.Id == model.Id);
                if (phrase != null)
                {
                    phrase.word = model.Word;
                    phrase.definition = model.Definition;
                    var response = await _phraseService.UpdateAsync(phrase);
                    return JsonResponseModel.Success(response);
                }
                else
                {
                    return JsonResponseModel.Error("Không tồn tại câu hỏi này", 404);
                }
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
        }

        /// <summary>
        /// Lấy ra danh sách có phân trang, search key: readingId 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("lists")]
        [Authorize]
        public async Task<JsonResponseModel> Search([FromBody] SearchReadingRequest model)
        {
            try
            {
                var search = model.Search;
                int readingId = search.GetValueByKey<int>("readingId");
                Func<Phrase, bool> condition = x => x.IsDeleted == 0 && x.fk_readingId == readingId;
                var list = _phraseService.GetListPaged(model.PageIndex, model.PageSize, condition, "CreatedDate", true);
                return JsonResponseModel.Success(list);
            }
            catch (Exception ex)    
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
        }

        /// <summary>
        /// Xóa 1 phrase trong reading
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<JsonResponseModel> Delete(int id)
        {
            try
            {
                var phrase = await _phraseService.GetFirstOrDefaultAsync(x => x.Id == id);
                if (phrase != null)
                {
                    phrase.IsDeleted = 1;
                    var response = await _phraseService.UpdateAsync(phrase);
                    return JsonResponseModel.Success($"Xóa thành công {id}");
                }
                else
                {
                    return JsonResponseModel.Error("Không tồn tại cụm từ này", 404);
                }
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
        }
    }
}
