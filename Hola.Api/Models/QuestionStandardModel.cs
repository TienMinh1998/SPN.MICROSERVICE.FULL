namespace Hola.Api.Models
{
    public class QuestionStandardModel
    {
        public string English { get; set; }
        public string Phonetic { get; set; }
        public string MeaningEnglish { get; set; }
        public string MeaningVietNam { get; set; }
        public string Note { get; set; }
        public int Pk_QuestionStandard_Id { get; set; }
        public bool Tick { get; set; }
    }

    public class UpdateQuestionStandardModel : QuestionStandardModel
    {
        public int Id { get; set; }
    }
}
