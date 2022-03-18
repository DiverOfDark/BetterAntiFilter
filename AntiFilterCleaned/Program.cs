using AntiFilterCleaned;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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