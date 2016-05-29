using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PinBuster.Data
{
    public class Utilities
    {
        HttpClient client;
        static string url_base = "https://pinbusterapi.azurewebsites.net/api/";

        public Utilities() {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }
        public async Task<String> MakeGetRequest(string api_path)
        {
            string r ="";

            var uri = new Uri(string.Format(url_base + api_path, string.Empty));
            System.Diagnostics.Debug.WriteLine(uri);

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"				ERROR {0}", ex.Message);
                
            }
            return null;
        }
        public static async Task<string> MakePostRequest(string url, string data/*, string cookie*/, bool isJson = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            
            request.Method = "POST";
            //request.Headers["Cookie"] = cookie;
            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(data);
                writer.Flush();
                writer.Dispose();
            }

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();


            using (StreamReader sr = new StreamReader(respStream))
            {
                //Need to return this response 
                return sr.ReadToEnd();
            }
        }
    }
}
