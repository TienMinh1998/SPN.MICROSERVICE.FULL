using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Hola.Api.Service.UploadFile
{
    public abstract class BaseUploadFileService
    {
        protected  IWebHostEnvironment _webHostEnvironment;
        protected HttpContext _httpContext;
        
        public HttpContext GetHttpContext()
        {
            return _httpContext;
        }
        public IWebHostEnvironment GetIWebHostEnvironment()
        {
          return _webHostEnvironment;
        }
        public void SetWebHostEnvironment(IWebHostEnvironment value)
        {
            _webHostEnvironment = value;
        }
        public void SetHttpContext(HttpContext value)
        {
            _httpContext = value;
        }
    }
}
