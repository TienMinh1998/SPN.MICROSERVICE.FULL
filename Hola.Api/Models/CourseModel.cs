using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Hola.Api.Models
{
    public class CourseModel
    {
        public string Title { get; set; }     // tiêu đề của khóa học
        public string Code { get; set; }       // Mã khóa học
        public string Target { get; set; }     // Mục tiêu của khóa học
        public string Content { get; set; }    // Nội dung của khóa học
        public string CoursImage { get; set; } // Thêm ảnh cho khóa học
    }

    public class UpdateCourseModel 
    {
        public string Title { get; set; }     // tiêu đề của khóa học
        public string Target { get; set; }     // Mục tiêu của khóa học
        public string Content { get; set; }    // Nội dung của khóa học
        public string CoursImage { get; set; } // Thêm ảnh cho khóa học
        public int Pk_coursId { get; set; } 
    }


    public class CourseModelRequest : CourseModel
    {
        public IFormFile file { get; set; }
    }
}
