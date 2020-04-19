using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PadelInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder => { 
           

               webBuilder.UseStartup<Startup>()
               .UseUrls("http://127.0.0.1:38000")

               .ConfigureAppConfiguration((builderContext, config) =>
               {
                   IWebHostEnvironment env = builderContext.HostingEnvironment;

                   config.SetBasePath(Directory.GetCurrentDirectory());
                   config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                   config.AddEnvironmentVariables();
               });

           });
           
               
    }
}
