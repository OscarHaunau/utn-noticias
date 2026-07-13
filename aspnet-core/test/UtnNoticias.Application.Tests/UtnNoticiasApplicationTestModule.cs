using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UtnNoticias.News;
using Volo.Abp.Modularity;

namespace UtnNoticias;

[DependsOn(
    typeof(UtnNoticiasApplicationModule),
    typeof(UtnNoticiasDomainTestModule)
    )]
public class UtnNoticiasApplicationTestModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		// En los tests no llamamos a NewsAPI real para no depender de internet ni de la API key.
				// Libro: p.106: reemplazamos la dependencia real por una falsa para no llamar internet en tests.
		context.Services.Replace(ServiceDescriptor.Transient<INewsService, FakeNewsService>());
	}
}
