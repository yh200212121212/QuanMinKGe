using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace QuanMinKGe
{
    /*
     * NetConnect: Provide functions based on HTTP
     */
    class NetConnect
    {
        /*
         * HttpGet: Implement GET method in HTTP
         * @Param url: website url
         * @Param dateStr: get parameters
         */
        public static async Task<string> HttpGet(string url)
        {
            // set url
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET"; // set method
            request.ContentType = "text/html;charset=UTF-8";    // set contentType
            request.UserAgent = "Mozilla/5.0";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();  // send request
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retstring = await myStreamReader.ReadToEndAsync();
            myStreamReader.Close();
            myResponseStream.Close();

            return retstring;
        }

        /*
         * HttpDownloadFile: file download function
         * @Param url: file url
         * @Param path: save file path
         */
        public static async Task<string> HttpDownloadFile(string url, string path)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;  // set url
            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;    // send request
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            Stream responseStream = response.GetResponseStream();
            Stream stream = new FileStream(path, FileMode.Create);  // create local file
            //byte[] bArr = new byte[1024];   // string buffer
            //int size = responseStream.Read(bArr, 0, (int)bArr.Length);  // read from response
            //while (size > 0)
            //{
            //    stream.Write(bArr, 0, size);    // write to file
            //    size = responseStream.Read(bArr, 0, (int)bArr.Length);
            //}
            await responseStream.CopyToAsync(stream);
            stream.Close();
            responseStream.Close();
            return path;    // return file absolute path
        }
    }
}
