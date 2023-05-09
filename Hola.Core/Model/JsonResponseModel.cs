using Hola.Core.Common;
using System.Collections.Generic;

namespace Hola.Core.Model
{
    public class JsonResponseModel
    {
        public int Status { set; get; }
        public object Data { set; get; }
        public string Message { set; get; }

        public static JsonResponseModel Response(int status, object data, string message)
        {
            return new JsonResponseModel() { Data = data, Message = message, Status = status };
        }
        public static JsonResponseModel Success(object data)
        {
            return new JsonResponseModel() { Data = data, Message = "SUCCESS", Status = 200 };
        }
        public static JsonResponseModel Success()
        {
            return new JsonResponseModel() { Data = null, Message = "SUCCESS", Status = 200 };
        }
        public static JsonResponseModel Success(object data, string _message)
        {
            return new JsonResponseModel() { Data = data, Message = _message, Status = 200 };
        }
        public static JsonResponseModel Error(string _message, int _code)
        {
            return new JsonResponseModel() { Data = null, Message = _message, Status = _code };
        }
        public static JsonResponseModel Error(object data, int _code, string _message)
        {
            return new JsonResponseModel() { Data = data, Message = _message, Status = _code };
        }

        public static JsonResponseModel SERVER_ERROR()
        {
            return new JsonResponseModel() { Data = new List<string>(), Message = SystemParam.MSG_SERVER_ERROR, Status = SystemParam.SERVER_ERROR_CODE };
        }

        public static JsonResponseModel SERVER_ERROR(string message)
        {
            return new JsonResponseModel() { Data = new List<string>(), Message = message, Status = SystemParam.SERVER_ERROR_CODE };
        }
    }
}