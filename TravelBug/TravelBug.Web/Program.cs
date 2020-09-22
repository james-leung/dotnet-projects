using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TravelBug.Context;
using TravelBug.Entities.UserData;

namespace TravelBug
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {
          var context = services.GetRequiredService<TravelBugContext>();
          var userManager = services.GetRequiredService<UserManager<AppUser>>();
          context.Database.Migrate();
          Seed.SeedData(context, userManager).Wait();
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred during migration.");
        }


        host.Run();
      }
    }
    //public static void Main(string[] args)
    //{
    //    CreateHostBuilder(args).Build().Run();
    //}

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
              // .UseUrls("http://0.0.0.0:5000");
            });
  }
}
