using UtnNoticias.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace UtnNoticias.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class UtnNoticiasController : AbpControllerBase
{
    protected UtnNoticiasController()
    {
        LocalizationResource = typeof(UtnNoticiasResource);
    }
}
