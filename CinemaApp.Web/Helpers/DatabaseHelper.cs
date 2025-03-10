using CinemaApp.Data;
using CinemaApp.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Helpers
{
    public static class DatabaseHelper
    {
        public static void ResetDatabase(CinemaDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
        }

        public static void SeedDatabase(IServiceProvider services, CinemaDbContext dbContext)
        {
            DatabaseSeeder.SeedRoles(services);
            DatabaseSeeder.AssignAdminRole(services);

            var movieCount = dbContext.Movies.Count();

            if (movieCount == 0)
            {
                dbContext.Movies.AddRange(new MovieConfiguration().SeedMovies());
                dbContext.SaveChanges();
            }
        }
    }
}
