using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Extensions.Publishers.Logger;
using Slackbot.Net.Extensions.Publishers.Slack;

namespace FplBot.ConsoleApps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults( (webBuilder) =>
                {
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                    {
                        webBuilder.UseUrls($"http://*:{80}");
                    }
                    
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((s,c) =>
                {
                    c.AddEnvironmentVariables();
                    c.AddJsonFile("appsettings.Local.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSlackbotWorker(hostContext.Configuration)
                        .AddSlackPublisherBuilder()
                        .AddLoggerPublisherBuilder()
                        .AddFplBot(hostContext.Configuration.GetSection("fpl"))
                        .BuildRecurrers();
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("LogicalHandler")))
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}][{Level:u3}] {Message:lj} ({SourceContext}){NewLine}{Exception}"));
    }
}
