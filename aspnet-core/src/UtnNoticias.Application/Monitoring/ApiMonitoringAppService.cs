using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace UtnNoticias.Monitoring;

// Libro: p.35: el calculo de estadisticas queda separado en un servicio de aplicacion.
public class ApiMonitoringAppService : UtnNoticiasAppService, IApiMonitoringAppService
{
	private readonly IRepository<ApiAccessLog, Guid> _apiAccessLogRepository;

	public ApiMonitoringAppService(IRepository<ApiAccessLog, Guid> apiAccessLogRepository)
	{
		_apiAccessLogRepository = apiAccessLogRepository;
	}

	public async Task<ApiMonitoringDto> GetStatsAsync()
	{
		// Estadistica basica del monitoreo pedido por la catedra.
				// Libro: p.63 muestra consultas con LINQ/Where sobre datos de EF Core.
		var queryable = await _apiAccessLogRepository.GetQueryableAsync();
		var accesses = await AsyncExecuter.ToListAsync(queryable);

		return new ApiMonitoringDto
		{
			TotalAccesses = accesses.Count,
			TotalErrors = accesses.Count(x => x.HasError),
			AverageMilliseconds = accesses.Count == 0 ? 0 : accesses.Average(x => x.DurationMilliseconds)
		};
	}
}
