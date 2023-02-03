namespace Hola.Api.Service.ExcelServices.DataConfig
{
    public class HeaderOfTable
    {
        public string TextAlign { get; set; }
        public int Location { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public int ColumnSpan { get; set; } = 0;
        public int Width { get; set; }
    }
}
