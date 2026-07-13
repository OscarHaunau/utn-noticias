using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using UtnNoticias.Monitoring;
using Xunit;

namespace UtnNoticias.Alerts;

// Libro: p.105 explica que las pruebas ayudan a evitar errores y validar funcionalidad.
public class AlertAppServiceTests : UtnNoticiasApplicationTestBase
{
	private readonly IAlertAppService _alertAppService;
	private readonly IApiMonitoringAppService _monitoringAppService;

	public AlertAppServiceTests()
	{
		_alertAppService = GetRequiredService<IAlertAppService>();
		_monitoringAppService = GetRequiredService<IApiMonitoringAppService>();
	}

	[Fact]
	public async Task Debe_Crear_Alerta_De_Noticias()
	{
		var alert = await _alertAppService.CreateAsync(new CreateNewsAlertDto
		{
			SearchText = " inteligencia artificial "
		});

		alert.SearchText.ShouldBe("inteligencia artificial");
		alert.IsActive.ShouldBeTrue();
	}

	[Fact]
	public async Task Debe_Ejecutar_Alertas_Y_Guardar_Notificaciones()
	{
		await _alertAppService.CreateAsync(new CreateNewsAlertDto
		{
			SearchText = "utn"
		});

		var creadas = await _alertAppService.RunAlertsAsync();
		var notifications = await _alertAppService.GetMyNotificationsAsync();

		creadas.ShouldBe(2);
		notifications.Count.ShouldBe(2);
		notifications.All(x => x.Title.Contains("utn")).ShouldBeTrue();
	}

	[Fact]
	public async Task Debe_Devolver_Notificaciones_Persistidas_Sin_Buscar_De_Nuevo()
	{
		await _alertAppService.CreateAsync(new CreateNewsAlertDto
		{
			SearchText = "backend"
		});
		await _alertAppService.RunAlertsAsync();

		var statsAntes = await _monitoringAppService.GetStatsAsync();
		var notifications = await _alertAppService.GetMyNotificationsAsync();
		var statsDespues = await _monitoringAppService.GetStatsAsync();

		notifications.ShouldNotBeEmpty();
		statsDespues.TotalAccesses.ShouldBe(statsAntes.TotalAccesses);
	}
}
