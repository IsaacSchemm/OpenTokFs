using ISchemm.OpenTokFs;
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

            var archives = await Requests.Archive.ListAsync(credentials, new OpenTokPagingParameters());
            Console.WriteLine(archives.count);
            foreach (var a in archives.items)
            {
                Console.WriteLine(a.id);
                Console.WriteLine(DateTimeOffset.FromUnixTimeMilliseconds(a.createdAt));
            }
        }
    }
}
