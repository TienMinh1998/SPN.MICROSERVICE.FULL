namespace Hola.Api.Requests.News
{
    public class NewsAddingRequest
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}
