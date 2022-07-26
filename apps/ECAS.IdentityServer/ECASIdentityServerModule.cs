using ECAS.AdministrationService.EntityFrameworkCore;
using ECAS.IdentityService.EntityFrameworkCore;
using ECAS.Shared.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;

namespace ECAS.IdentityServer;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAccountWebIdentityServerModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AdministrationServiceEntityFrameworkCoreModule),
    typeof(IdentityServiceEntityFrameworkCoreModule),
    typeof(ECASHostingModule)
    )]
public class ECASIdentityServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        Microsoft.Extensions.Configuration.IConfiguration configuration = context.Services.GetConfiguration();
        Configure<AbpBundlingOptions>(options =>
        {
            _ = options.StyleBundles.Configure(BasicThemeBundles.Styles.Global, bundle =>
            {
                _ = bundle.AddFiles("/global-styles.css");
            });
        });
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql();
        });
        Configure<AbpAuditingOptions>(options =>
        {
            //options.IsEnabledForGetRequests = true;
            options.ApplicationName = "AuthServer";
        });
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));
            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "ECAS:";
        });
        IDataProtectionBuilder dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("ECAS");
        if (!hostingEnvironment.IsDevelopment())
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            _ = dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "ECAS-Protection-Keys");
        }
        _ = context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                _ = builder.WithOrigins(configuration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        IdentityModelEventSource.ShowPII = true;
        IApplicationBuilder app = context.GetApplicationBuilder();
        Microsoft.AspNetCore.Hosting.IWebHostEnvironment env = context.GetEnvironment();
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }
        _ = app.UseAbpRequestLocalization();
        if (!env.IsDevelopment())
        {
            _ = app.UseErrorPage();
        }
        _ = app.UseCorrelationId();
        _ = app.UseStaticFiles();
        _ = app.UseRouting();
        _ = app.UseCors();
        _ = app.UseAuthentication();
        _ = app.UseUnitOfWork();
        _ = app.UseIdentityServer();
        _ = app.UseAuthorization();
        _ = app.UseAuditing();
        _ = app.UseAbpSerilogEnrichers();
        _ = app.UseConfiguredEndpoints();
    }
}
