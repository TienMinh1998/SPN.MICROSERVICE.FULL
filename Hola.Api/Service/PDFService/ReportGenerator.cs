using Hola.Api.Service.PDFService.Bases;
using Hola.Api.Service.PDFService.Models;
using Hola.Api.Service.PDFService112;
using System;
using System.Collections.Generic;

namespace Hola.Api.Service
{
    abstract class ReportGenerator
    {
        protected Pager paper = new Pager();
        protected List<object> _listData;
        private PDFService112.PDFService pdfService;
        public Pager GenerateReport(List<object> listData)
        {
            _listData = listData;
            GenerateHeader();
            GenerateData();
            GenerateFooter();
            // Cài đặt khổ giấy ngang
            paper.ORIENTED_PAPER = ORIENTED.vertical;
            return paper;
        }
        protected virtual void GenerateHeader()
        {
            // setting header infomation
            pdfService = new PDFService112.PDFService();
            var listHeader = new List<TextItem>
            {
               new TextItem
                {
                    Value = "SỞ GIÁO DỤC VÀ ĐÀO TẠO HÀ NỘI",
                    FontStyle = pdfService.GetFont(10)
                },
               new TextItem
                {
                    Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM",
                    FontStyle= pdfService.GetFont(10)
                },
               new TextItem
                {
                   Value = "Công ty Viết Tiến Hùng",
                    FontStyle = pdfService.GetFont(9)
                },
               new TextItem
                {
                    Value = "Độc lập - Tự do - Hạnh phúc",
                    FontStyle = pdfService.GetFont(9)
                },
               new TextItem
                {
                    Value = "-------------"
                },
               new TextItem
                {
                    Value = "-------------"
                }
            };
            paper.Header = listHeader;
        }
        protected abstract void GenerateData();
        protected virtual void GenerateFooter()
        {
            // Setting footter data Collection
            pdfService = new PDFService112.PDFService();
            var listHeader = new List<TextItem>
            {
               new TextItem
                {
                    Value = "NGƯỜI HỌC VIÊN",
                    FontStyle = pdfService.GetFont(12)
                },
               new TextItem
                {
                    Value = "TM. HỘI ĐỒNG QUẢN TRỊ CÔNG TY",
                    FontStyle= pdfService.GetFont(12)
                },
                new TextItem
                {
                    Value = "(Kí và ghi rõ họ tên)",
                    FontStyle= pdfService.GetFont(10)
                },
                new TextItem
                {
                    Value = "CHỮ KÍ CỦA GIÁM ĐỐC",
                    FontStyle= pdfService.GetFont(10)
                },
                 new TextItem
                {
                    Value = "\n\n\n\n\n\n ",
                    FontStyle= pdfService.GetFont(10)
                },
                new TextItem
                {
                    Value = "\n\n\n\n\n\n Nguyễn Viết Minh Tiến",
                    FontStyle= pdfService.GetFont(10)
                }
            };
            paper.Footter = listHeader;
        }
    }
}
