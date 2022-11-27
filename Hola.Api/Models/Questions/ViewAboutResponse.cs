using System.Collections.Generic;

namespace Hola.Api.Models.Questions
{
    public class ViewAboutResponse
    {
        public int TotalAll { get; set; }
        public int TotalCountToday { get; set; }
        public List<QuestionModel> ListQuestion { get; set; }
    }
}
