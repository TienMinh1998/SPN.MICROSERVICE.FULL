using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.GoogleCloudStorage
{
    public interface IUploadFileGoogleCloudStorage
    {
        string UploadFile(string path, string useId, string fileName, string credentials_path, string folder,string contentType);
        string UploadImage(string path, string useid, string fileName, string credentials_path);
        string UploadDocument(string path, string useid, string fileName, string credentials_path);

        string UploadImage(Stream path, string useid, string fileName, string credentials_path);

    }
}
