using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace CalculationVacationSystem.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    string keyvaulturi = Environment.GetEnvironmentVariable("VaultUri");
                    if (keyvaulturi != null)
                    {
                        var keyVaultEndpoint = new Uri(keyvaulturi);
                        config.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
                    }
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
