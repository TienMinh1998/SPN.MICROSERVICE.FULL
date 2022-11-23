using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Collections.Generic;
using Hola.Core.Common;

namespace Hola.Core.Helper
{
    public class ServiceClient
    {
        private string _baseAddress;
        private string? _userId;
        public ServiceClient(string? userId, string baseUrl)
        {
            _baseAddress = baseUrl;
            _userId = userId;
        }


    }
}
