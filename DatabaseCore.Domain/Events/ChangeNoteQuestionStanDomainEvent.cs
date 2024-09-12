using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Events
{
    public class ChangeNoteQuestionStanDomainEvent : INotification
    {
        public string Note { get; set; }
        public int QuestionID { get; set; }

        public ChangeNoteQuestionStanDomainEvent(int questionId, string note)
        {
            Note = note;
            QuestionID = questionId;
        }
    }
}
