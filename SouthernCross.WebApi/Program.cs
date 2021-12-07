using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using SouthernCross.Core.Extensions;
using System;

namespace SouthernCross.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WithDefaults().CreateLogger();

            try
            {
                Log.Logger.ForContext<Program>().Information("Starting");
                var host = CreateHostBuilder(args).Build();
                host.Run();
                Log.Logger.ForContext<Program>().Information("Stopping");
            }
            catch (Exception e)
            {
                Log.Logger.ForContext<Program>().Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}
