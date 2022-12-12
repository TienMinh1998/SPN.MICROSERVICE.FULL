namespace Hola.Api.Models.Questions
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public int Category_Id { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public string ImageSource { get; set; }
        public string audio { get; set; }

    }
}
