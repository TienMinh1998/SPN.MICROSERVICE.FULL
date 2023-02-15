using System.Collections.Generic;

namespace Hola.Api.Service.IText7
{
    public class InputPage
    {
        public InputPage()
        {
            this.HeaderInput = new HeaderModel();
            this.BodyModel = new BodyModel();
            this.FootterModel = new FootterModel();
        }
        public HeaderModel HeaderInput { get; set; }
        public BodyModel BodyModel { get; set; }
        public FootterModel FootterModel { get; set; }
    }
}
