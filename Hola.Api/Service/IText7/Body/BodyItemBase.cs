using Hola.Api.Service.IText7.DefaultConfig;

namespace Hola.Api.Service.IText7.Body
{
    public abstract class BodyItemBase
    {
        public string Title { get; set; }
        public BODY_TYPE TYPE { get; set; }
    }
}
