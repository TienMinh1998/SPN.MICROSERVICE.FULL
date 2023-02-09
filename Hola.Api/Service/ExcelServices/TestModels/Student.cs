namespace Hola.Api.Service.ExcelServices.TestModels
{
    public class Student : IStudent
    {
        public int STT { get; set; }
        public string Session { get; set; }
        public string Vicle { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string Time { get; set; }
        public string url { get; set; }
    }


    public class Student1 : IStudent
    {
        public int STT { get; set; }
        public string Session { get; set; }
    }

    public interface IStudent
    {

    }
}
