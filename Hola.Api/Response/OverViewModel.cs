namespace Hola.Api.Response
{
    public class OverViewModel
    {
        public int Total { get; set; }          // Tổng số câu
        public int TotalToday { get; set; }
        public int TotalLearned { get; set; }    // Tổng số câu đã học
        public int TotalNotLearnd { get; set; }  // Tổng số câu chưa học
    }
}
