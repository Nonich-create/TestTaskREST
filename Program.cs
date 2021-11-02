using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using TestTask.DAL.Data;
using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.Extensions.Logging;

namespace TestTaskREST
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Log.Logger = new LoggerConfiguration()
           
             .Enrich.FromLogContext()
             .WriteTo.Console(new RenderedCompactJsonFormatter())
             .WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/log.ndjson")
             .WriteTo.Seq("http://localhost:5341")
             .CreateBootstrapLogger();
           
            var host = CreateHostBuilder(args).Build();
           
            try
            {
                Log.Information("Starting up");
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<Context>();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                  //  webBuilder.UseUrls("http://*:80");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
