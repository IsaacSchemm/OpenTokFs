using OpenTokFs;
using OpenTokFs.RequestTypes;
using System;
using System.Threading.Tasks;

using Requests = OpenTokFs.Requests;

namespace Example
{
    class OpenTokCredentials : IOpenTokCredentials
    {
        public int ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var credentials = new OpenTokCredentials();

            Console.Write("Enter your API key: ");
            credentials.ApiKey = int.Parse(Console.ReadLine());

            Console.Write("Enter your API secret: ");
            credentials.ApiSecret = Console.ReadLine();

            var archives = await Requests.Archive.ListAllAsync(credentials, 2);
            foreach (var a in archives)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(a));
            }
        }
    }
}
