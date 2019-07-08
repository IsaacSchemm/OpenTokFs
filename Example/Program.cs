using OpenTokFs;
using OpenTokFs.RequestTypes;
using System;
using System.Threading.Tasks;

using Requests = OpenTokFs.Requests;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter your API key: ");
            int apiKey = int.Parse(Console.ReadLine());

            Console.Write("Enter your API secret: ");
            string apiSecret = Console.ReadLine();

            var credentials = new OpenTokCredentials(apiKey, apiSecret);

            var archives = await Requests.Archive.ListAllAsync(credentials, 10);
            Console.WriteLine(archives.Length);

            foreach (var a in archives)
            {
                Console.WriteLine(a.id);
                Console.WriteLine(a.name);
                Console.WriteLine(a.GetCreationTime() + " " + a.GetDuration());
                Console.WriteLine();
            }
        }
    }
}
