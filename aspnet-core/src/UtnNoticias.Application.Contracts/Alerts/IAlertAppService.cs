using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace UtnNoticias.Alerts;

public interface IAlertAppService : IApplicationService
{
	Task<NewsAlertDto> CreateAsync(CreateNewsAlertDto input);
	Task<ICollection<NewsNotificationDto>> GetMyNotificationsAsync();
	Task<int> RunAlertsAsync();
}
