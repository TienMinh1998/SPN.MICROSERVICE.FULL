using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPNApplication.Commnands
{
    class AddQuestionStandCommand : IRequest<bool>
    {
        public string English { get; set; }          // Từ bằng tiếng anh
        public string Phonetic { get; set; }         // Phiên tâm
        public string MeaningEnglish { get; set; }   // Nghĩa tiếng anh
        public string MeaningVietNam { get; set; }   // Nghĩa tiếng việt
        public string Note { get; set; }             // Ghi chú
        public int user_id { get; set; }
    }
}
