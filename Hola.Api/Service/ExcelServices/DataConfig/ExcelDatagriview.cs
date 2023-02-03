using Hola.Api.Service.ExcelServices.Enum;
using System.Collections.Generic;

namespace Hola.Api.Service.ExcelServices.DataConfig
{
    public class ExcelDatagriview
    {
        public string Content { get; set; }
        public string SubContent { get; set; }
        public string FontWeigh { get; set; }
        public int StartColumnOfContent { get; set; }
        public int Size { get; set; }
        public int SizeSub { get; set; }
        public string FontFamily { get; set; }
        public string Align { get; set; }
        public object Data { get; set; }
        public string BacgroundColor { get; set; }
        public string TextHeaderColor { get; set; }
        public string TextColor { get; set; }
        public int MergeColumnTitle { get; set; }
        public int MarginTop { get; set; }
        public DATAGRIVIEW_TYPE Type { get; set; }
        public List<string> InfomationText { get; set; }
        public List<HeaderOfTable> Headers { get; set; }
        public List<FootterConfig> Footers { get; set; }
        public List<ImageProperties> Images { get; set; }
     
    }

    public class ExcelDatagriviewBuilder
    {
        private ExcelDatagriview data = new ExcelDatagriview();
        public ExcelDatagriview Build => data;
        public ExcelDatagriviewBuilder SetMarginTop(int value)
        {
                data.MarginTop = value;
                return this;
        }
        public ExcelDatagriviewBuilder SetType(DATAGRIVIEW_TYPE value)
        {
            data.Type = value;
            return this;
        }
        public ExcelDatagriviewBuilder SetTitle(string value,int mergeCol)
        {
            data.Content = value;
            data.MergeColumnTitle = mergeCol;
            return this;
        }
        public ExcelDatagriviewBuilder SetSubTitle(string value)
        {
            data.SubContent = value;
            return this;
        }
        public ExcelDatagriviewBuilder SetDecorate()
        {
            data.Size = 16;
            data.SizeSub = 10;
            data.FontFamily = "Times new roman";
            data.StartColumnOfContent = 1;
            data.Align = "center";
            data.FontWeigh = "";
            return this;
        }
        public ExcelDatagriviewBuilder SetImage(List<ImageProperties> list)
        {
            data.Images = list;
            return this;
        }
        public ExcelDatagriviewBuilder SetInfomation(List<string> list)
        {
            data.InfomationText= list;
            return this;
        }
        public ExcelDatagriviewBuilder SetCollection(object collection)
        {
            data.Data = collection;
            return this;
        }
        public ExcelDatagriviewBuilder SetHeaderConfiguration(List<HeaderOfTable> list)
        {
            data.Headers = list;
            return this;
        }
        public ExcelDatagriviewBuilder SetFooter(List<FootterConfig> listContentFooter)
        {
            data.Footers = listContentFooter;
            return this;
        }

    }



}
