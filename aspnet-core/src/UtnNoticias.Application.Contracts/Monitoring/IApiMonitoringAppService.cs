using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace UtnNoticias.Monitoring;

public interface IApiMonitoringAppService : IApplicationService
{
	Task<ApiMonitoringDto> GetStatsAsync();
}
