using System;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KycNoteRequest
    {
        public Guid KycId { get; set; }
        public string UserId { get; set; }
        public string rejectReason { get; set; }
    }

    public class KycNoteDataRequest
    {
        public string KycId { get; set; }
        public string UserId { get; set; }        
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }

    public class NoteListModel
    {
        public int Id { get; set; }
        public Guid KycId { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }
        public string CreatedById { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
