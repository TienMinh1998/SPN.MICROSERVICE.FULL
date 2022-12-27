using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Hola.Api.Models
{
    public class AddTopicModel
    {
        public int fK_Course_Id { get; set; }
        public string image { get; set; }
        public string englishContent { get; set; }
        public string vietNamContent { get; set; }
    }

    public class UpdateTopicModel : AddTopicModel {
        public int pK_Topic_Id { get; set; }
        public IFormFile file { get; set; }
    }
}
