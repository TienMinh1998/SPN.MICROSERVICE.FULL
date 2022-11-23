using System.Collections.Generic;

namespace Hola.Core.Model
{
    public class SettingModel
    {
        public string Provider { set; get; }
        public string Connection { set; get; }
        public List<string> Connections { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserServiceUrl { set; get; }
        public string ConfigServiceUrl { set; get; }
        public string CommonServiceUrl { set; get; }
        public string TransferServiceUrl { get; set; }

    }

}