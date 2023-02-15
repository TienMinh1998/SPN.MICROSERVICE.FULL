using Hola.Api.Service.IText7.DefaultConfig;
using System.Collections.Generic;

namespace Hola.Api.Service.IText7.Body
{
    public class BodyInfomation : BodyItemBase, IBody
    {

        public string Infomation { get; set; }
        public string Url { get; set; }

        public BODY_TYPE GetBodyType()
        {
            this.TYPE = BODY_TYPE.TEXT_AND_IMAGE;
            return this.TYPE;
        }

        public List<object> GetCollection()
        {
            return new List<object>();
        }

        public string GetStudentInfomation()
        {
            return Infomation;
        }

        public string GetTitle()
        {
            return this.Title;
        }
    }
}
