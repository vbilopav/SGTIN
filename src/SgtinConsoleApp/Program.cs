using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SgtinConsoleApp
{
    class Program
    {
        private static HttpClient client = new HttpClient(); // https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/

        static void Main(string[] args)
        {
            var findWhat = "Milka Oreo";
            var results = new List<ulong>();
            client.BaseAddress = new Uri("http://localhost:64535");

            using (var file = new StreamReader("data/tags.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    var url = $"/api/Sgtin/{line}";
                    Console.Write($"GET {url} ... ");
                    var result = client.GetAsync(url).Result;
                    Console.WriteLine(result.StatusCode);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<JObject>(result.Content.ReadAsStringAsync().Result);
                        var serial = data["serialReference"].Value<uint>();
                        url = $"/api/Sgtin/{line}/{findWhat}";
                        Console.Write($"GET {url} ... ");
                        var status = client.GetAsync(url).Result.StatusCode;
                        Console.WriteLine(result.StatusCode);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            results.Add(serial);
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("*** Results ***");
            Console.WriteLine($"Found {results.Count} valid serials that refer to {findWhat} with following serial numbers: {string.Join(", ", results.ToArray())}");
        }
    }
}
