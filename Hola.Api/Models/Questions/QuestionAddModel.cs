namespace Hola.Api.Models.Questions
{
    public class QuestionAddModel
    {
        public int Category_Id { get; set; }
        public string QuestionName { get; set; }
        public string Answer { get; set; }
        public string ImageSource { get; set; }
        public int fk_userid { get; set; }
        public string Note { get; set; }
    }
}
