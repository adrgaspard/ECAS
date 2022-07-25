using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace ECAS.Shared.Hosting;

[DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpDataModule),
        //typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule),
        //typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
public class ECASHostingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
        Configure<AbpDbConnectionOptions>(options =>
        {
            _ = options.Databases.Configure("AdministrationService", database =>
            {
                _ = database.MappedConnections.Add("AbpAuditLogging");
                _ = database.MappedConnections.Add("AbpPermissionManagement");
                _ = database.MappedConnections.Add("AbpSettingManagement");
                _ = database.MappedConnections.Add("AbpFeatureManagement");
            });
            _ = options.Databases.Configure("IdentityService", database =>
            {
                _ = database.MappedConnections.Add("AbpIdentity");
                _ = database.MappedConnections.Add("AbpIdentityServer");
            });
        });
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
        });
    }
}

