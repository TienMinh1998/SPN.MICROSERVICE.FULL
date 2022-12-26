using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Hola.Api.Models
{
    public class AddTopicModel
    {
        public int FK_Course_Id { get; set; }
        public string Image { get; set; }
        public string EnglishContent { get; set; }
        public string VietNamContent { get; set; }
    }

    public class UpdateTopicModel : AddTopicModel {
        public int PK_Topic_Id { get; set; }
        public IFormFile File { get; set; }
    }
}
