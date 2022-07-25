using ECAS.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ECAS.IdentityService.EntityFrameworkCore;

internal class IdentityServiceDbContextFactory : IDesignTimeDbContextFactory<IdentityServiceDbContext>
{
    public IdentityServiceDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IdentityServiceDbContext> builder = new DbContextOptionsBuilder<IdentityServiceDbContext>().UseNpgsql(GetConnectionStringFromConfiguration());
        return new IdentityServiceDbContext(builder.Options);
    }

    private static string GetConnectionStringFromConfiguration()
    {
        return BuildConfiguration().GetConnectionString(IdentityServiceDbProperties.ConnectionStringName);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(
                    Path.Combine(
                        Directory.GetParent(Directory.GetCurrentDirectory())?.Parent!.FullName!,
                        $"host{Path.DirectorySeparatorChar}{Consts.GlobalSuiteName}.IdentityService.HttpApi.Host"
                    )
                )
                .AddJsonFile("appsettings.json", false);
        return builder.Build();
    }
}

