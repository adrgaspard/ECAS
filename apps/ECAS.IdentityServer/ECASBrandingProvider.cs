using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ECAS.IdentityServer;

[Dependency(ReplaceServices = true)]
public class ECASBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ECAS";
}
