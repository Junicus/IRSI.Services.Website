using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IRSI.Services.Website
{
    public class Program
    {
        private static readonly Dictionary<string, string> defaults =
            new Dictionary<string, string>
            {
                {WebHostDefaults.EnvironmentKey, "development" }
            };

        public static void Main(string[] args)
        {
            Console.Title = "IRSI.Services.Website";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddInMemoryCollection(defaults)
                .AddJsonFile("hosting.json", optional: true)
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddCommandLine(args)
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
