using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace UtnNoticias;

[Dependency(ReplaceServices = true)]
public class UtnNoticiasBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "UtnNoticias";
}
