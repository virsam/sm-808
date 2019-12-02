using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DrumMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hb, cb) =>
                    cb.RegisterModule(new AutofacModule(hb.Configuration)))
                .ConfigureLogging((hbc, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    if (hbc.HostingEnvironment.IsDevelopment())
                    {
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, port, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                            listenOptions.UseHttps();
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });

        private const int port = 5001;
    }
}
