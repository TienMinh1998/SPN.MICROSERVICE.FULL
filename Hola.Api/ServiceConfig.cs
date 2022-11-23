using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api
{
    public class ServiceConfig
    {
        public Dictionary<string, string> Services { get; set; }
        public class Blob
        {
            public string EnvFolder { get; set; }
            public string AccessToken { get; set; }
            public string SubFolder { get; set; }
        }

        public ServiceConfig()
        {
            BlobSettings = new Blob();
        }
        public Blob BlobSettings { get; set; }
    }
}
