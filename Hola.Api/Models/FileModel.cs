namespace Hola.Api.Model
{
    public class FileModel
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int Size { get; set; }
        public string Extension { get; set; }
    }

    public class FileModelResponse
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
