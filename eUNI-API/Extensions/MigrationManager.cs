using eUNI_API.Data;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Extensions;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        appContext.Database.Migrate();

        return webApp;
    }
}