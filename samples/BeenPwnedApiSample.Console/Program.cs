using System.Threading;
using BeenPwned.Api;

namespace BeenPwnedApiSample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new BeenPwnedClient("BeenPwnedApiSample.Console"))
            {

                System.Console.WriteLine("-- All breaches --");
                var breaches = client.GetAllBreaches().Result;

                foreach (var breach in breaches)
                    System.Console.WriteLine($"{breach.Name}");

                // Sleep for a while to prevent hitting the rate limit
                Thread.Sleep(3000);

                System.Console.WriteLine();
                System.Console.WriteLine("-- All breaches for adobe.com --");
                var breachesForDomain = client.GetAllBreaches("adobe.com").Result;

                foreach (var breach in breachesForDomain)
                    System.Console.WriteLine($"{breach.Name}");

                Thread.Sleep(3000);

                System.Console.WriteLine();
                System.Console.WriteLine("-- All pastes for account test@example.com --");
                var pastes = client.GetPastesForAccount("test@example.com").Result;

                foreach (var paste in pastes)
                    System.Console.WriteLine($"{paste.Source}");

                Thread.Sleep(3000);

                System.Console.WriteLine();
                System.Console.WriteLine("-- All data classes --");
                var dataclasses = client.GetAllDataClasses().Result;

                foreach (var dataclass in dataclasses)
                    System.Console.WriteLine($"{dataclass}");

                Thread.Sleep(3000);

                System.Console.WriteLine();
                System.Console.WriteLine("-- Password pwned? (GET) --");
                var isPwnedGet = client.GetPwnedPassword("Foo").Result;

                System.Console.WriteLine($"{isPwnedGet}");

                Thread.Sleep(3000);

                System.Console.WriteLine();
                System.Console.WriteLine("-- Password pwned? (POST) --");
                var isPwnedPost = client.GetPwnedPassword("Foo", sendAsPostRequest: true).Result;

                System.Console.WriteLine($"{isPwnedPost}");

                System.Console.WriteLine("-----------------");
                System.Console.WriteLine("That's all folks!");

                System.Console.ReadKey();
            }
        }
    }
}