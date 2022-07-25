using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace ECAS.AdministrationService;

public class AdministrationServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public AdministrationServiceDataSeedContributor()
    {
    }

    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
}
