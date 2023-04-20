using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Hola.Api.Service.ExcelServices.TestModels;
using System.Reflection;
using Hola.Api.Models;
using Hola.Core.Utils;
using Hola.Api.Service.BaseServices;
using System.Threading.Tasks;
using Hola.Api.Service.IText7;
using iText.Layout.Properties;
using Hola.Api.Service.IText7.Body;
using Microsoft.AspNetCore.Http;
using Hola.Api.Service.BAImportExcel;
using Hola.Core.Model;
using System.Xml;

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
        [HttpPost("ImportXml")]
        public async Task<JsonResponseModel> ImportXml(IFormFile file)
        {
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var value = ReadData<Student1>(tempFilePath, new List<string>
            {
                "id","name","age"
            });
            return JsonResponseModel.Success(value);
        }
        private List<T> ReadData<T>(string filePath, List<string> headerNames)
        {
            Type objectType = typeof(T);
            var properties = objectType.GetProperties().ToList();
            List<T> listAnyThing = new List<T>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeList studentNodes = xmlDoc.GetElementsByTagName(objectType.Name.ToLower());
            foreach (XmlNode studentNode in studentNodes)
            {
                List<MapDataConfig> values = new List<MapDataConfig>();
                foreach (var node in headerNames)
                {
                    object _value = studentNode[node].InnerText;
                    values.Add((new MapDataConfig()).SetColumnName(node).SetValue((_value != null) ? _value.ToString() : string.Empty));
                }
                var item = CreateNewIntanceByProperties<T>(values, properties);
                listAnyThing.Add(item);
            }
            return listAnyThing;
        }
        private T CreateNewIntanceByProperties<T>(List<MapDataConfig> data, List<PropertyInfo> properties)
        {
            try
            {
                var obj = (T)Activator.CreateInstance(typeof(T));
                int i = 0;
                foreach (var item in properties)
                {
                    string columnName = item.Name;
                    var cellValue = data.Where(x => x.ColumnName.ToLower() == columnName.ToLower()).FirstOrDefault();
                    if (cellValue != null)
                    {
                        if (string.IsNullOrEmpty(cellValue.Value))
                        {
                            i++;
                            continue;
                        }

                        if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type underlyingType = Nullable.GetUnderlyingType(item.PropertyType);
                            if (underlyingType == typeof(DateTime))
                            {
                                var dateTimeValue = cellValue.Value.ConvertStringToDateTime();
                                item.SetValue(obj, Convert.ChangeType(dateTimeValue, underlyingType));
                            }
                            else
                            {

                                object underlyingValue = Convert.ChangeType(cellValue.Value, underlyingType);
                                object convertedValue = Activator.CreateInstance(item.PropertyType, underlyingValue);
                                item.SetValue(obj, Convert.ChangeType(convertedValue, underlyingType));
                            }
                        }
                        else
                        {
                            if (item.PropertyType == typeof(DateTime))
                            {
                                var dateTimeValue = cellValue.Value.ConvertStringToDateTime();
                                item.SetValue(obj, Convert.ChangeType(dateTimeValue, item.PropertyType));
                            }
                            else
                            {
                                item.SetValue(obj, Convert.ChangeType(cellValue.Value, item.PropertyType));
                            }
                        }
                        i++;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
