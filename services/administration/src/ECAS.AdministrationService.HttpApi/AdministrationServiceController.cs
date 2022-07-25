using ECAS.AdministrationService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ECAS.AdministrationService;

public abstract class AdministrationServiceController : AbpControllerBase
{
    protected AdministrationServiceController()
    {
        LocalizationResource = typeof(AdministrationServiceResource);
    }
}
