namespace Hola.Api.Service.BAImportExcel
{
    public class MapDataConfig
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }

        public MapDataConfig SetColumnName(string name)
        {
            this.ColumnName = name;
            return this;
        }

        public MapDataConfig SetValue(string value)
        {
            this.Value = value;
            return this;
        }
    }



}
