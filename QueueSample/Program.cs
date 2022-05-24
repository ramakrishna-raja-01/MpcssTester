using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace QueueSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("./logs/paymentlogs.log", rollingInterval: RollingInterval.Day)
               .CreateLogger();

            Log.Debug("Application started");
            CreateHostBuilder(args).Build().Run();

        }

        /* public static void Main(string[] args)
         {
             Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                 .Enrich.FromLogContext()
                 .WriteTo.Debug(
                     LogEventLevel.Verbose,
                     "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}"
                 )
                 .WriteTo.File(
                     formatter: new CompactJsonFormatter(),
                     path: "./logs/MpcssLogs.json",
                     restrictedToMinimumLevel: LogEventLevel.Information,
                     buffered: true,
                     rollingInterval: RollingInterval.Day
                 )

                 .CreateLogger();

             try
             {
                 Log.Information("Starting web host");
                 CreateWebHostBuilder(args).Build().Run();
             }
             catch (Exception ex)
             {
                 Log.Fatal(ex, "Host terminated unexpectedly");
             }
             finally
             {
                 Log.CloseAndFlush();
             }
         }*/

        /*public static Microsoft.AspNetCore.Hosting.IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders(); // removes all providers from LoggerFactory
                    logBuilder.AddConsole();
                     logBuilder.AddDebug();
                     logBuilder.AddTraceSource("Information, ActivityTracing"); 
                     // Add Trace listener provider
                    */

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
