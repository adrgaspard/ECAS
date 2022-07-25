using ECAS.AdministrationService;
using ECAS.AdministrationService.EntityFrameworkCore;
using ECAS.IdentityService;
using ECAS.IdentityService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ECAS.Shared.DbMigrator;

[DependsOn(
        typeof(AbpAutofacModule),
        typeof(AdministrationServiceEntityFrameworkCoreModule),
        typeof(AdministrationServiceApplicationContractsModule),
        typeof(IdentityServiceEntityFrameworkCoreModule),
        typeof(IdentityServiceApplicationContractsModule)
        )]
public class ECASDbMigratorModule : AbpModule
{
}

