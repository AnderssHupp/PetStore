using Microsoft.EntityFrameworkCore;

namespace Loja.Data
{
    public static class DataExtensions
    {
        //to migrate our db every time we start the app
        public static void MigrateDb( this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.
                GetRequiredService<PetStoreContext>();

            dbContext.Database.Migrate();
        }
    }
}
