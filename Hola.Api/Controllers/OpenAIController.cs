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

using System.Reflection;
using static iTextSharp.text.pdf.AcroFields;
using Hola.Api.Models;
using Hola.Core.Utils;
using Hola.Api.Service.BaseServices;
using System.Threading.Tasks;
using Hola.Api.Service.IText7;
using iText.Layout.Properties;
using Hola.Api.Service.IText7.Body;
using Hola.Api.Authorize;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Hola.Api.Service.BAImportExcel;
using Hola.Core.Model;

namespace Hola.Api.Controllers
{
    public class OpenAIController : ControllerBase
    {

        private LibPDfService text7Service;


        private readonly DapperBaseService _dapper;
        public OpenAIController(DapperBaseService dapper)
        {

            text7Service = new LibPDfService();
            _dapper = dapper;
        }

        [HttpGet("CreatePdfUsingItext7")]
        public async Task<IActionResult> IText7()
        {
            List<QuestionStandardModel> data = new List<QuestionStandardModel>();
            for (int i = 0; i < 15; i++)
            {
                data.Add(new QuestionStandardModel
                {
                    English = "ENGLISH",
                    MeaningEnglish = "TEST",
                    MeaningVietNam = "Thử Nghiệm",
                    Note = "Không",
                    Phonetic = "123",
                    Pk_QuestionStandard_Id = 1,
                    Tick = true
                });
            }
            var headerConfig = new List<HeaderConfig>
                {
                    new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "English",
                        DisplayColumnName= "Từ vựng",
                        ordinalNumber=1,
                        Width=10f,
                    },
                     new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "Phonetic",
                        DisplayColumnName= "Phiên âm",
                        ordinalNumber=2,
                        Width=10f,
                    },
                    new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "MeaningEnglish",
                        DisplayColumnName= "Nghĩa tiếng anh",
                        ordinalNumber=3,
                        Width=10f,
                    },
                    new HeaderConfig
                    {

                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "MeaningVietNam",
                        DisplayColumnName= "Nghĩa tiếng việt",
                        ordinalNumber=4,
                        Width=10f,
                    },
                    new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "Note",
                        DisplayColumnName= "Ghi chú",
                        ordinalNumber=5,
                        Width=10f,
                    },
                    new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "Pk_QuestionStandard_Id",
                        DisplayColumnName= "ID",
                        ordinalNumber=6,
                        Width=10f,
                    },
                     new HeaderConfig
                    {
                        TextAlignment = TextAlignment.CENTER,
                        ColumnName= "Tick",
                        DisplayColumnName= "Tick",
                        ordinalNumber=7,
                        Width=10f,
                    }
                };
            InputPage inputPDf = new InputPage();


            IBodyCollection bodyCollection = new IBodyCollection();
            bodyCollection.TYPE = Service.IText7.DefaultConfig.BODY_TYPE.COLLECTION;
            bodyCollection.collection = new List<object>();
            bodyCollection.Title = "I. BÁO CÁO THỨ NHẤT";
            data.ForEach(x => bodyCollection.collection.Add(x));

            BodyInfomation bodyInfomation = new BodyInfomation();
            bodyInfomation.Infomation = "";
            bodyInfomation.Infomation = " 1. Họ và tên     : Nguyễn Viết Minh Tiến";
            bodyInfomation.Infomation += "\n 2. Mã học viên   : 1234-5678-999";
            bodyInfomation.Infomation += "\n 3. Ngày sinh     : 10.3.1998";
            bodyInfomation.Infomation += "\n 4. Mã khóa học   : 10.3.1998.000999";
            bodyInfomation.Infomation += "\n 5. Hạng đào tạo  : B2";
            bodyInfomation.Infomation += "\n 5. Cơ sở đào tạo : Trung tâm Bình Anh";

            inputPDf.HeaderInput.HeaderDefault = new List<string>
            {
                "SỞ GIÁO DỤC ĐÀO TẠO THÁI BÌNH \n Trung tâm Bình Anh",
                "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM \n Độc lập - Tự do - Hạnh phúc",
                "-------------",
                "-------------"
            };
            inputPDf.HeaderInput.SubTitles = new List<string>
            {
                "(Ngày báo cáo : 10.2.2023)",
                "(Người làm : Nguyễn Viết Minh Tiến)"
            };
            inputPDf.HeaderInput.MainTitles = new List<string> {
                "BÁO CÁO CHI TIẾT QUÁ TRÌNH HỌC LÁI XE CỦA HỌC VIÊN"
            };

            inputPDf.BodyModel.BodyItems.Add(bodyInfomation);
            inputPDf.BodyModel.BodyItems.Add(bodyCollection);
            inputPDf.BodyModel.BodyItems.Add(bodyCollection);


            text7Service.CreateHeaderConfig(headerConfig);
            var file = text7Service.CreateDocument(inputPDf);
            return File(file, "application/pdf", "Example.pdf");
        }

        [HttpPost("ImportExcel")]
        public JsonResponseModel ImportExcel(IFormFile file)
        {
            ExcelLibImport service = new ExcelLibImport();
            List<string> headerName = new List<string>()
            {
                "STT","Code", "Name", "BirthDay","CCCD","NgayCap","NoiCap","Sex",
                "Phone","Email","Adress","Adress1","RegisterType","Rank",
                "RankRegister","GPLX","NgayCapGPLX", "NgayHetHan","NoiCapGPLX",
                "YearOfDriver","Km"
            };

            var input = (new ExcelImportRequestBuilder())
                               .SetStartRow(3)
                               .SetWorkSheetIndex(1)
                               .SetPaddingBottom(0)
                               .SetFile(file)
                               .SetHeaderColumn(headerName)
                               .Build;
            var response = service.ConvertToList<StudentTestModel>(input);

            return JsonResponseModel.Success(response);
        }

        public class Person
        {
            public string Name { get; set; }
            public DateTime CreateDate { get; set; }
            public int Age { get; set; }
        }
    }
}
