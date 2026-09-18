using Microsoft.EntityFrameworkCore;
using MyApp.Domain;
using MyApp.Features.Auth.Shared;

namespace MyApp.Infrastructure;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, IPasswordHasher passwordHasher, IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(passwordHasher);
        ArgumentNullException.ThrowIfNull(configuration);

        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var adminUserOptions = configuration.GetSection(AdminUserOptions.SectionName).Get<AdminUserOptions>() ?? new AdminUserOptions();
        var now = DateTimeOffset.UtcNow;

        var adminUser = User.Create(
            adminUserOptions.Username,
            passwordHasher.Hash(adminUserOptions.Password),
            "Admin",
            "Admin",
            now,
            true);

        dbContext.Users.Add(adminUser);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
