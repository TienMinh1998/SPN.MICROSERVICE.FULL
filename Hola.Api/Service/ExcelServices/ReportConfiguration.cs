using Hola.Api.Service.ExcelServices.DataConfig;
using System.Collections.Generic;

namespace Hola.Api.Service.ExcelServices
{
    public class ReportConfiguration
    {
        public List<HeaderOfPager> ExHeader { get; set; }
        public Body Body { get; set; }
        public List<TextPlus> Texts { get; set; }
        public bool IsPortrait { get; set; }


        public ReportConfiguration()
        {
            Body = new Body();
            ExHeader = new List<HeaderOfPager>();
            Texts = new List<TextPlus>();
        }
    }

}
