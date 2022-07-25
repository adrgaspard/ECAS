using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volo.Abp;

namespace ECAS.Shared.DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IAbpApplicationWithInternalServiceProvider? application = await AbpApplicationFactory.CreateAsync<ECASDbMigratorModule>(options =>
        {
            _ = options.Services.ReplaceConfiguration(_configuration);
            options.UseAutofac();
            _ = options.Services.AddLogging(c => c.AddSerilog());
        });
        await application.InitializeAsync();
        await application.ServiceProvider.GetRequiredService<ECASDbMigrationService>().MigrateAsync(cancellationToken);
        await application.ShutdownAsync();
        _hostApplicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

