using Hola.Api.Service.ExcelServices.TestModels;
using Hola.Api.Service.PDFService;
using Hola.Api.Service.PDFService.Bases;
using Hola.Api.Service.PDFService.Models;
using Hola.Api.Service.PDFService112;
using iTextSharp.text;
using System.Collections.Generic;

namespace Hola.Api.Service
{
    class PDFReport : ReportGenerator
    {

        private string studentImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmFowc1FYog7leGBz-9gdKO8n4cmlwXMqzkA&usqp=CAU";
        protected override void GenerateData()
        {
            // fake dataCollection
            // Data of student infomation
            paper.MainTitle = "BÁO CÁO CHI TIẾT THỰC HÀNH LÁI XE NĂM HỌC 2023";
            BodyInfomation bodyInfomation = new BodyInfomation();
            bodyInfomation.UrlImage = studentImage;
            bodyInfomation.Texts = new List<string>
                    {
                        "1. Họ và tên     : Nguyễn Viết Minh Tiến",
                        "2. Mã học viên   : 123456789.999",
                        "3. Ngày sinh     : 10-03-1999",
                        "4. Mã khóa học   : KH.99912",
                        "5. Hạng đào tạo  : B2",
                        "6. Cơ sở đào tạo : Trung tâm giáo dục nghề nghiệp và sát hạch lái xe Hà Nội"
                    };
            bodyInfomation.TYPE = BODY_ITEMTYPE.TEXT_AND_IMAGE;

            // Data Collection
            BodyCollection collection = new BodyCollection();
            collection.Collection = new TableConfig();
            collection.Collection.HeaderColor = new BaseColor(1, 190, 250); // màu xanh nước biển
            //collection.Collection.HeaderConfig = new List<HeaderConfig>
            //{
            //        new HeaderConfig
            //            {
            //                ColumnName = "STT",
            //                DisplayColumnName = "STT",
            //                Width = 40f,
            //                ordinalNumber = 1,
            //                Align = Element.ALIGN_CENTER,
            //            },
            //        new HeaderConfig
            //        {
            //            ColumnName = "Session",
            //            DisplayColumnName = "Phiên học",
            //            Width = 200f,
            //            ordinalNumber = 2,
            //            Align= Element.ALIGN_CENTER,
            //        },
            //        new HeaderConfig
            //        {
            //            ColumnName = "Vicle",
            //            DisplayColumnName = "Phương Tiện",
            //            Width = 100f,
            //            ordinalNumber = 3,
            //            Align= Element.ALIGN_LEFT
            //        },
            //        new HeaderConfig
            //        {
            //            ColumnName = "LoginTime",
            //            DisplayColumnName = "Thời gian đăng nhập",
            //            Width = 100f,
            //            ordinalNumber = 4,
            //            Align= Element.ALIGN_LEFT
            //        },
            //        new HeaderConfig
            //        {
            //            ColumnName = "LogoutTime",
            //            DisplayColumnName = "Thời gian đăng xuất",
            //            Width = 100f,
            //            ordinalNumber = 5,
            //            Align = Element.ALIGN_LEFT
            //        },
            //        new HeaderConfig
            //        {
            //            ColumnName = "Time",
            //            DisplayColumnName = "Thời gian",
            //            Width = 100f,
            //            ordinalNumber = 6,
            //            Align = Element.ALIGN_LEFT
            //        },
            //        new HeaderConfig
            //        {
            //            ColumnName = "url",
            //            DisplayColumnName = "Ảnh",
            //            Width =50f,
            //            ordinalNumber = 7,
            //            Align= Element.ALIGN_RIGHT
            //        }
            //};
            collection.Collection.FootterConfig = new List<FootterItem>
                    {
                        new FootterItem
                        {
                            ColSpan= 5,
                            TextAlign= Element.ALIGN_CENTER,
                            Value = "1.200.000 VNĐ"
                        },
                        new FootterItem
                        {
                            ColSpan= 2,
                            TextAlign= Element.ALIGN_RIGHT,
                            Value = "Đạt"
                        },
                        new FootterItem
                        {
                            ColSpan= 7,
                            TextAlign= Element.ALIGN_CENTER,
                            Value = "Nguyễn Viết Minh Tiến"
                        },
                    };
            collection.Collection.Data = _listData;
            collection.Title = "II. BẢNG DỮ LIỆU 1";
            collection.TYPE = BODY_ITEMTYPE.COLLECTION;

            // Add database to paper
            paper.Body = new List<IBody>();
            paper.Body.Add(bodyInfomation);
            paper.Body.Add(collection);
            //paper.Body.Add(collection);


        }
    }
}
