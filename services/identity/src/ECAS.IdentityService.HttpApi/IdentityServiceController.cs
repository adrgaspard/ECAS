using ECAS.IdentityService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ECAS.IdentityService;

public abstract class IdentityServiceController : AbpControllerBase
{
    protected IdentityServiceController()
    {
        LocalizationResource = typeof(IdentityServiceResource);
    }
}
