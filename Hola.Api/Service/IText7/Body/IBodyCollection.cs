using Hola.Api.Service.IText7.DefaultConfig;
using System.Collections.Generic;

namespace Hola.Api.Service.IText7.Body
{
    public class IBodyCollection : BodyItemBase, IBody
    {
        public List<object> collection { get; set; }
        public BODY_TYPE GetBodyType()
        {
            return this.TYPE;
        }

        public List<object> GetCollection()
        {
            return collection;
        }

        public string GetStudentInfomation()
        {
            return "";
        }

        public string GetTitle()
        {
            return Title;
        }
    }
}
