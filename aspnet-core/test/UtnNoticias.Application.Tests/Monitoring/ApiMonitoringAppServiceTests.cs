using System.Threading.Tasks;
using Shouldly;
using UtnNoticias.News;
using Xunit;

namespace UtnNoticias.Monitoring;

// Libro: p.105 relaciona estas pruebas con pruebas automaticas de comportamiento.
public class ApiMonitoringAppServiceTests : UtnNoticiasApplicationTestBase
{
	private readonly IUtnNoticiasService _newsAppService;
	private readonly IApiMonitoringAppService _monitoringAppService;

	public ApiMonitoringAppServiceTests()
	{
		_newsAppService = GetRequiredService<IUtnNoticiasService>();
		_monitoringAppService = GetRequiredService<IApiMonitoringAppService>();
	}

	[Fact]
	public async Task Debe_Mostrar_Estadisticas_De_Accesos_A_La_Api()
	{
		await _newsAppService.Search("economia");
		await _newsAppService.Search("deportes");

		var stats = await _monitoringAppService.GetStatsAsync();

		stats.TotalAccesses.ShouldBeGreaterThanOrEqualTo(2);
		stats.TotalErrors.ShouldBe(0);
		stats.AverageMilliseconds.ShouldBeGreaterThanOrEqualTo(0);
	}
}
