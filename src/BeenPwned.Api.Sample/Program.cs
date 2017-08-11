using System;
using System.Threading;

namespace BeenPwned.Api.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BeenPwnedClient("BeenPwned.Api.SampleApp");

            Console.WriteLine("-- All breaches --");
            var breaches = client.GetAllBreaches().Result;

            foreach (var breach in breaches)
                Console.WriteLine($"{breach.Name}");

            Thread.Sleep(3000);

            Console.WriteLine();
            Console.WriteLine("-- All breaches for adobe.com --");
            var breachesForDomain = client.GetAllBreaches(domain: "adobe.com").Result;

            foreach (var breach in breachesForDomain)
                Console.WriteLine($"{breach.Name}");

            Thread.Sleep(3000);

            Console.WriteLine();
            Console.WriteLine("-- All pastes for account test@example.com --");
            var pastes = client.GetPastesForAccount("test@example.com").Result;

            foreach (var paste in pastes)
                Console.WriteLine($"{paste.Source}");

            Thread.Sleep(3000);

            Console.WriteLine();
            Console.WriteLine("-- All data classes --");
            var dataclasses = client.GetAllDataClasses().Result;

            foreach (var dataclass in dataclasses)
                Console.WriteLine($"{dataclass}");

            Thread.Sleep(3000);

            Console.WriteLine();
            Console.WriteLine("-- Password pwned? (GET) --");
            var isPwnedGet = client.GetPwnedPassword("Foo").Result;

            Console.WriteLine($"{isPwnedGet}");

            Thread.Sleep(3000);

            Console.WriteLine();
            Console.WriteLine("-- Password pwned? (POST) --");
            var isPwnedPost = client.GetPwnedPassword("Foo", sendAsPostRequest:true).Result;

            Console.WriteLine($"{isPwnedPost}");

            Console.ReadKey();
        }
    }
}