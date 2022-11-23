namespace Hola.Api.Models
{
    public class ReviewAgentApplicationRequest
    {
        public int Id { get; set; }
        public byte StatusId { get; set; }
        public string Reason { get; set; }
    }
}
