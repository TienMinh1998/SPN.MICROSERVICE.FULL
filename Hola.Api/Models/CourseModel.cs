using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Hola.Api.Models
{
    public class CourseModel
    {
    
        public string title { get; set; }     // tiêu đề của khóa học

        public string code { get; set; }       // Mã khóa học

        public string target { get; set; }     // Mục tiêu của khóa học

        public string content { get; set; }    // Nội dung của khóa học
    }

    public class UpdateCourseModel 
    {
     
        public string title { get; set; }      // Tiêu đề của khóa học
 
        public string target { get; set; }     // Mục tiêu của khóa học
    
        public string content { get; set; }    // Nội dung của khóa học
  
        public int pk_coursId { get; set; }    // Pk_courseId

        public IFormFile file { get; set; }    // file 
    }


    public class CourseModelRequest : CourseModel
    {
        public IFormFile file { get; set; }
    }
}
