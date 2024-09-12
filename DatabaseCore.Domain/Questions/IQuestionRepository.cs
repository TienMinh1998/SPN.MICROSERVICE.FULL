

namespace DatabaseCore.Domain.Questions;

public interface IQuestionRepository
{
    QuestionStandard AddQuestion(QuestionStandard questionStandard);
    QuestionStandard UpdateQuestion(QuestionStandard questionStandard);
    QuestionStandard DeleteQuestion(QuestionStandard questionStandard);
}
