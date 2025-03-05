using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.SeedData
{
    public static class DBInitializerSeedData
    {
        public static async Task InitializeDatabaseAsync(TemplateDbContext isoDbContext)
        {
            if (await isoDbContext.Roles.AnyAsync())
                return;

            var roles = new Role[]
            {
                new Role { RoleName = "Admin" },
                new Role { RoleName = "TQM" },
                new Role { RoleName = "QSO" },
                new Role { RoleName = "CO1" },
                new Role { RoleName = "CO2" },
                new Role { RoleName = "CO3" },
                new Role { RoleName = "CO4" },
                new Role { RoleName = "CO5" },
                new Role { RoleName = "AUD" }
            };

            await isoDbContext.Roles.AddRangeAsync(roles);
            await isoDbContext.SaveChangesAsync();
        }
    }
}
