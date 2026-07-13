using UtnNoticias.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace UtnNoticias.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(UtnNoticiasEntityFrameworkCoreModule),
    typeof(UtnNoticiasApplicationContractsModule)
    )]
public class UtnNoticiasDbMigratorModule : AbpModule
{
}
