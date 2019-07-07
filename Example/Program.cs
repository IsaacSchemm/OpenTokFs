using ISchemm.OpenTokFs;
using ISchemm.OpenTokFs.RequestTypes;
using System;
using System.Threading.Tasks;

using Requests = ISchemm.OpenTokFs.Requests;

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

            var archives = await Requests.Archive.ListAsync(credentials, new OpenTokPagingParameters(), "2_MX40NTk3MzI3Mn5-MTUyMzU1NDQwOTA5MX5LcHN2ditxaVNTZWlRenpVVElodUFuQ09-fg");
            Console.WriteLine(archives.count);
            foreach (var i in archives.items)
            {
                Console.WriteLine(i.status);
            }
        }
    }
}
