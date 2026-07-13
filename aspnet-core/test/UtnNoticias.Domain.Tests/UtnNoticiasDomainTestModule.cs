using UtnNoticias.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace UtnNoticias;

[DependsOn(
    typeof(UtnNoticiasEntityFrameworkCoreTestModule)
    )]
public class UtnNoticiasDomainTestModule : AbpModule
{

}
