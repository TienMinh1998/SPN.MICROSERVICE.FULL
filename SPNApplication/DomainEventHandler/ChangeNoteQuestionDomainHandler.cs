using MediatR;

namespace SPNApplication.DomainEventHandler
{
    public class ChangeNoteQuestionDomainHandler : INotificationHandler<ChangeNoteQuestionStanDomainEvent>
    {
        public Task Handle(ChangeNoteQuestionStanDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Question {notification.QuestionID} is change Note ");
            return Task.CompletedTask;
        }
    }
}
