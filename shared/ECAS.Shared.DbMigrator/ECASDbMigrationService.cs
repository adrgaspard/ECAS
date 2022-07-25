using ECAS.AdministrationService.EntityFrameworkCore;
using ECAS.IdentityService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace ECAS.Shared.DbMigrator;

public class ECASDbMigrationService : ITransientDependency
{
    private readonly IDataSeeder _dataSeeder;
    private readonly ILogger<ECASDbMigrationService> _logger;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ECASDbMigrationService(ILogger<ECASDbMigrationService> logger, IDataSeeder dataSeeder, IUnitOfWorkManager unitOfWorkManager)
    {
        _logger = logger;
        _dataSeeder = dataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating Host side...");
        await MigrateAllDatabasesAsync(cancellationToken);
        await SeedDataAsync();
        _logger.LogInformation("Migration completed!");
    }

    private async Task MigrateAllDatabasesAsync(CancellationToken cancellationToken)
    {
        using (IUnitOfWork? uow = _unitOfWorkManager.Begin(true))
        {
            await MigrateDatabaseAsync<AdministrationServiceDbContext>(cancellationToken);
            await MigrateDatabaseAsync<IdentityServiceDbContext>(cancellationToken);
            await uow.CompleteAsync(cancellationToken);
        }
        _logger.LogInformation($"All databases have been successfully migrated.");
    }

    private async Task MigrateDatabaseAsync<TDbContext>(CancellationToken cancellationToken) where TDbContext : DbContext, IEfCoreDbContext
    {
        _logger.LogInformation($"Migrating {typeof(TDbContext).Name.RemovePostFix("DbContext")} database...");
        TDbContext? dbContext = await _unitOfWorkManager.Current.ServiceProvider.GetRequiredService<IDbContextProvider<TDbContext>>().GetDbContextAsync();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    private async Task SeedDataAsync()
    {
        await _dataSeeder.SeedAsync(new DataSeedContext()
            .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, "admin@abp.io")
            .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, "1q2w3E*")
        );
    }
}

