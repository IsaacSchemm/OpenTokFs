using OpenTokFs;
using OpenTokFs.RequestTypes;
using System;
using System.Collections.Generic;
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

            await Requests.Session.SetLayoutClassesAsync(credentials, "sessionIdGoesHere", new Dictionary<string, IEnumerable<string>>
            {
                ["streamIdGoesHere"] = new[] { "full" }
            });
        }
    }
}
