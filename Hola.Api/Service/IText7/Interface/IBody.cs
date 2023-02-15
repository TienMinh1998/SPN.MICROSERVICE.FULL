using Hola.Api.Service.IText7.DefaultConfig;
using System.Collections.Generic;

namespace Hola.Api.Service.IText7;

public interface IBody
{
    public string GetTitle();
    public BODY_TYPE GetBodyType();
    public string GetStudentInfomation();

    public List<object> GetCollection();

}
