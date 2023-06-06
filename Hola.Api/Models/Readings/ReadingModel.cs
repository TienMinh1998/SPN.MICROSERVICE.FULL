using System;

namespace Hola.Api.Models.Readings
{
    public class ReadingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Definetion { get; set; }
        public string Image { get; set; }
        public string Translate { get; set; }
        public string Status { get; set; }
        public string TaskName { get; set; }
        public int Band { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsDeleted { get; set; }
    }
}
