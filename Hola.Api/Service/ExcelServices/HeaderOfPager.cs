namespace Hola.Api.Service.ExcelServices
{
    public class HeaderOfPager
    {

        public string TopContent { get; set; }
        public string BottomContent { get; set; }
        public string FontWeigh { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int Size { get; set; }
        public int ColumnSpan { get; set; }
        public string FontFamily { get; set; }
        public string Align { get; set; }
        public bool Visible { get; set; } = true;
    }

    public class HeaderBuilder
    {
        private HeaderOfPager header = new HeaderOfPager();

        public HeaderOfPager build => header;
        public HeaderBuilder SetTopContent(string content)
        {
            header.TopContent = content;
            return this;
        }
        public HeaderBuilder SetBottomContent(string content)
        {
            header.BottomContent = content;
            return this;
        }
        public HeaderBuilder SetFontWeigh(string font)
        {
            header.FontWeigh = font;
            return this;
        }
        public HeaderBuilder SetAlign(string align)
        {
            header.Align = align;
            return this;
        }
        public HeaderBuilder SetStartRow(int row)
        {
            header.StartRow = row;
            return this;
        }
        public HeaderBuilder SetStartColumn(int row)
        {
            header.StartColumn = row;
            return this;
        }
        public HeaderBuilder SetColumnSpan(int span)
        {
            header.ColumnSpan = span;
            return this;
        }
        public HeaderBuilder SetSize(int size)
        {
            header.Size = size;
            return this;
        }
        public HeaderBuilder SetDecorate()
        {
            header.Visible = true;
            header.FontWeigh = "";
            header.FontFamily = "Times new roman";
            header.Align = "Center";
            return this;
        }
    }
}
