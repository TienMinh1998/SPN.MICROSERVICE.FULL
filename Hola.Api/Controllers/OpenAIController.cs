using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Hola.Api.Service;
using System.Collections.Generic;
using System.Linq;
using Hola.Api.Service.ExcelServices.TestModels;
using Newtonsoft.Json;
using Hola.Api.Service.PDFService;
using Hola.Api.Service.PDFService112;
using System.Reflection;
using static iTextSharp.text.pdf.AcroFields;
using Hola.Api.Service.PDFService.Models;
using Hola.Api.Service.PDFService.Bases;
using Hola.Api.Models;
using Hola.Core.Utils;
using Hola.Api.Service.BaseServices;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class OpenAIController : ControllerBase
    {
        private PDFService pdfService;
        private readonly DapperBaseService _dapper;
        public OpenAIController(DapperBaseService dapper)
        {
            pdfService = new PDFService();
            _dapper = dapper;
        }

        [HttpGet]
        [Route("GenatePDF")]
        public async Task<IActionResult> GeneratePdf()
        {

            var queryGetId = "SELECT \"PK_Topic_Id\" FROM usr.topic where lower(Trim(\"EnglishContent\"))=lower(Trim('water shortage'));";
            var topicId = _dapper.QueryFirstOrDefault<int>(queryGetId);

            if (topicId == 0) return null;
            string query = "SELECT a.\"Pk_QuestionStandard_Id\",  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\",  a.\"MeaningVietNam\", a.\"Note\" " +
                "  FROM  (public.\"QuestionStandards\" q " +
               "\r\ninner join usr.\"QuestionStandardDetail\" qd on q.\"Pk_QuestionStandard_Id\"" +
               $" = qd.\"QuestionID\" ) a\r\ninner join usr.topic tq on tq.\"PK_Topic_Id\" = a.\"TopicID\"\r\nwhere a.\"TopicID\" = {topicId}  order by a.\"Pk_QuestionStandard_Id\"";

            var response = await _dapper.GetAllAsync<QuestionStandardModel>(query.AddPadding(1, 30));
            var data = response.ToList();

            List<Person> persons = new List<Person>();

            for (int i = 0; i < 20; i++)
            {
                persons.Add(new Person
                {
                    Age = i,
                    CreateDate = DateTime.Now,
                    Name = "Nguyễn Viết Minh Tiến"
                });
            }

            List<object> results = new List<object>();
            persons.ForEach(x => results.Add(x));

            PDFReport pDFReport = new PDFReport();
            var paper = pDFReport.GenerateReport(results);
            var file = pdfService.GenratePDF(paper);
            return File(file, "application/pdf", "Example.pdf");
        }

        public class Person
        {
            public string Name { get; set; }
            public DateTime CreateDate { get; set; }
            public int Age { get; set; }
        }
    }
}
