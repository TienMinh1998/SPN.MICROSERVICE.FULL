
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Net.Http;

namespace Hola.Api.Service.PDFService112;

public class PDFService
{
    public string GenratePDF(string _html)
    {
        try
        {
            // Tạo đường dẫn
            string filename = DateTime.Now.ToString("ssddMMyyyy") + "Example.pdf";
            string fileURL = Path.Combine(Directory.GetCurrentDirectory(), $"PdfTemplate/{filename}").Trim();
            FileInfo file = new FileInfo(fileURL);
            if (file.Exists) file.Delete();

            return "";
        }
        catch (System.Exception ex)
        {
            throw;
        }
    }

    public byte[] GetImageFromUrl(string url)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                return httpClient.GetByteArrayAsync(url).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading image from URL: " + url + ", error: " + ex.Message);
                return new byte[0];
            }
        }
    }
}
