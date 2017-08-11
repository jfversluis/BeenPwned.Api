using System;

namespace BeenPwned.Api.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BeenPwnedClient("BeenPwned.Api.SampleApp");

            var breaches = client.GetBreaches().Result;

            Console.WriteLine("-- All breaches --");

            foreach (var breach in breaches)
                Console.WriteLine($"{breach.Name}");

            Console.WriteLine();
            Console.WriteLine("-- All pastes for account test@example.com --");
            var pastes = client.GetPastes("test@example.com").Result;

            foreach (var paste in pastes)
                Console.WriteLine($"{paste.Source}");

            Console.WriteLine();
            Console.WriteLine("-- All data classes --");
            var dataclasses = client.GetDataClasses().Result;

            foreach (var dataclass in dataclasses)
                Console.WriteLine($"{dataclass}");

            Console.ReadKey();
        }
    }
}