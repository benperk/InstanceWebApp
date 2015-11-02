using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using static System.Console;
using System.Net;
using System.Net.Http;

namespace InstanceWebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var instances = new string[] 
            {
                "b4ed1065a3557b5b4cb989f29e8ade38563ef1b7a3ef0dcfca258813f6b21147",
                "c496b3e3a90e9b94304a876ae040c5cb348b746eae04c0fc8b6c8e87373ef09c",
                "3b192aa8f3c7c04ebae20e2e12464a35b30e0620160f938a1b9faac82a271889"
            };

            var sitenames = new string[]
            {
                "W-1", "W-2", "W-n", "standard"
            };            

            foreach (var site in sitenames)
            {
                var url = new Uri("http://" + site + ".azurewebsites.net/areYouOK.html");

                foreach (var instance in instances)
                {
                    var response = GetFromInstance(url, instance).Result;
                    WriteLine("Answer from site: " + site + " instance: " + instance);
                    WriteLine(response.Content.ReadAsStringAsync().Result);
                    WriteLine();
                }
            }            
            ReadLine();
        }
        private static async Task<HttpResponseMessage> GetFromInstance(Uri url, string instanceId)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var httpClient = new HttpClient(handler))
                {
                    cookieContainer.Add(url, new Cookie("ARRAffinity", instanceId));
                    return await httpClient.GetAsync(url);
                }
            }
        }
    }
}
