using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FplBot.ConsoleApps
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Run(async context => { await context.Response.WriteAsync("Hello, World!"); });
        }
    }
}