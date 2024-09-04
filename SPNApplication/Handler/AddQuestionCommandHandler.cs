using Hola.Core.Helper;
using Hola.Core.Model;
using MediatR;
using Microsoft.Extensions.Hosting;
using SPNApplication.Commnands;
using SPNApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPNApplication.Handler
{
    public class AddQuestionCommandHandler : IRequestHandler<AddQuestionStandCommand, bool>
    {
        private readonly IDapperService _dapperService;
        public AddQuestionCommandHandler(IDapperService dapperService)
        {
            _dapperService = dapperService;
        }

        async Task<bool> IRequestHandler<AddQuestionStandCommand, bool>.Handle(AddQuestionStandCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //  Lấy ra audio của từ đó, nếu không lấy được audio thì mặc định để trống;
                // Nếu từ đó có dấu cách thì thôi không lấy audio nữa 
                int userid = request.user_id;
                string camAudio = string.Empty;
                string camPhonetic = string.Empty;
                string note = string.Empty;
                string camType = string.Empty;

                bool isBasicWord = request.English.Trim().Contains(" ");

                if (!isBasicWord)
                {
                    try
                    {
                        string word = request.English;
                        APICrossHelper api = new APICrossHelper();
                        // Chạy bất đồng bộ để lấy về của nghĩa tiếng việt
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
                string typeNote = "";
                typeNote = camType.Trim();
                if (camType.Trim().ToLower() == "adverb")
                {
                    typeNote = "(adv)";
                }
                else if (camType.Trim().ToLower() == "adjective")
                {
                    typeNote = "(adj)";
                }
                else if (camType.Trim().ToLower() == "noun")
                {
                    typeNote = "(n)";
                }
                else if (camType.Trim().ToLower() == "verb")
                {
                    typeNote = "(v)";
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
