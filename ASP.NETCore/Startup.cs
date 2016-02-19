using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;

namespace Algae.WebApp
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                System.Console.WriteLine(DateTime.Now.ToString());
                await context.Response.WriteAsync("head, body");
            });
        }
    }
}
