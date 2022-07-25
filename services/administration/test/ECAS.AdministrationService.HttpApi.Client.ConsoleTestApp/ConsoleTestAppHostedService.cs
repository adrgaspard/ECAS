using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace ECAS.AdministrationService;

public class ConsoleTestAppHostedService : IHostedService
{
    private readonly IConfiguration _configuration;

    public ConsoleTestAppHostedService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IAbpApplicationWithInternalServiceProvider application = await AbpApplicationFactory.CreateAsync<AdministrationServiceConsoleApiClientModule>(options =>
        {
            _ = options.Services.ReplaceConfiguration(_configuration);
            options.UseAutofac();
        });
        await application.InitializeAsync();

        ClientDemoService demo = application.ServiceProvider.GetRequiredService<ClientDemoService>();
        await demo.RunAsync();

        await application.ShutdownAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
