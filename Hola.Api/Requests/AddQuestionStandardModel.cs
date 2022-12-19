using System;

namespace Hola.Api.Requests
{
    public class AddQuestionStandardModel
    {
        public string English { get; set; }          // Từ bằng tiếng anh
        public string Phonetic { get; set; }         // Phiên tâm
        public string MeaningEnglish { get; set; }   // Nghĩa tiếng anh
        public string MeaningVietNam { get; set; }   // Nghĩa tiếng việt
        public string Note { get; set; }             // Ghi chú
    }
}
