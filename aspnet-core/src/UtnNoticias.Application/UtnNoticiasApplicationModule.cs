using Microsoft.Extensions.DependencyInjection;
using UtnNoticias.News;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace UtnNoticias;

[DependsOn(
    typeof(UtnNoticiasDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(UtnNoticiasApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class UtnNoticiasApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<UtnNoticiasApplicationModule>();
        });

        //se registra el servicio de noticias. Deberia registrarse solo, pero como me dio error lo incorporo aca
        context.Services.AddTransient<INewsService, NewsApiService>();
    }
}
