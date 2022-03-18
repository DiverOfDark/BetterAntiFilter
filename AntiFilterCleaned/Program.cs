using System.Net;
using AntiFilterCleaned;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseKestrel(t => t.Listen(IPAddress.Any, 80));
        var app = builder.Build();

        var afSvc = new AntiFilterService();

        app.Use(async (a, b) =>
        {
            foreach (var network in afSvc.Networks)
            {
                await a.Response.WriteAsync(network.ToString() + "\n");
            }

            if (false)
            {
                await b(a);
            }
        });

        app.Run();
    }
}