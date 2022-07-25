using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace ECAS.IdentityService;

public class IdentityServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public IdentityServiceDataSeedContributor()
    {
    }

    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
}
