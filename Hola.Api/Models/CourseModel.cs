using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Hola.Api.Models
{
    public class CourseModel
    {
        public string Title { get; set; }     // tiêu đề của khóa học
        public string Code { get; set; }       // Mã khóa học
        public string Target { get; set; }     // Mục tiêu của khóa học
        public string Content { get; set; }    // Nội dung của khóa học
    }
}
