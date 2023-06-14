using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Univali.Api.DbContexts;
using Microsoft.Extensions.Logging;

namespace Univali.Api.Extensions;

internal static class StartupHelperExtensions
{
   public static async Task ResetDatabaseAsync(this WebApplication app, ILogger logger)
   {
       using (var scope = app.Services.CreateScope())
       {
           try
           {
               var context = scope.ServiceProvider.GetService<CustomerContext>();
               if (context != null)
               {
                   await context.Database.EnsureDeletedAsync();
                   await context.Database.MigrateAsync();
               }
           }
            catch (Exception ex)
            {
               logger.LogError(ex, "An error occurred while migrating the database.");
           }
       }
   }
}
