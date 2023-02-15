using System.Collections.Generic;

namespace Hola.Api.Service.IText7;

public class BodyModel
{

    public BodyModel()
    {
        BodyItems = new List<IBody>();
    }
    public List<IBody> BodyItems { get; set; }
    public string CollectionTitle { get; set; }
}
