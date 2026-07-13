using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtnNoticias.Monitoring;
using UtnNoticias.News;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;

namespace UtnNoticias.Alerts;

// Libro: p.35: cada caso de uso queda en un servicio simple y testeable.
public class AlertAppService : UtnNoticiasAppService, IAlertAppService
{
		// Libro: p.52 explica EF Core como capa para guardar/consultar datos con clases C#.
	private readonly IRepository<NewsAlert, Guid> _alertRepository;
	private readonly IRepository<NewsNotification, Guid> _notificationRepository;
	private readonly IRepository<ApiAccessLog, Guid> _apiAccessLogRepository;
	private readonly INewsService _newsService;

	public AlertAppService(
		IRepository<NewsAlert, Guid> alertRepository,
		IRepository<NewsNotification, Guid> notificationRepository,
		IRepository<ApiAccessLog, Guid> apiAccessLogRepository,
		INewsService newsService)
	{
		_alertRepository = alertRepository;
		_notificationRepository = notificationRepository;
		_apiAccessLogRepository = apiAccessLogRepository;
		_newsService = newsService;
	}

	public async Task<NewsAlertDto> CreateAsync(CreateNewsAlertDto input)
	{
		// Cada alerta pertenece al usuario logueado.
		var alert = new NewsAlert(GuidGenerator.Create(), GetCurrentUserId(), input.SearchText);
		alert = await _alertRepository.InsertAsync(alert, autoSave: true);
		return ObjectMapper.Map<NewsAlert, NewsAlertDto>(alert);
	}

	public async Task<ICollection<NewsNotificationDto>> GetMyNotificationsAsync()
	{
		// Este metodo NO busca en NewsAPI. Solo devuelve lo persistido de la ultima semana.
		var ownerId = GetCurrentUserId();
		var from = Clock.Now.AddDays(-7);
		var queryable = await _notificationRepository.GetQueryableAsync();
		var notifications = await AsyncExecuter.ToListAsync(
			queryable
				.Where(x => x.OwnerId == ownerId && x.CreatedAt >= from)
				.OrderByDescending(x => x.CreatedAt)
		);

		return ObjectMapper.Map<List<NewsNotification>, ICollection<NewsNotificationDto>>(notifications);
	}

		// Libro: p.42: este metodo es asincronico porque consulta una API externa.
	public async Task<int> RunAlertsAsync()
	{
		// Simula la ejecucion asincronica: busca cada alerta activa y guarda notificaciones nuevas.
		var queryable = await _alertRepository.WithDetailsAsync(x => x.Notifications);
		var alerts = await AsyncExecuter.ToListAsync(queryable.Where(x => x.IsActive));
		var savedNotifications = 0;

		foreach (var alert in alerts)
		{
			var startedAt = Clock.Now;
			try
			{
				var articles = await _newsService.GetNewsAsync(alert.SearchText);
				foreach (var article in articles)
				{
					var beforeCount = alert.Notifications.Count;
					alert.AddNotification(article.Title, article.Url, Clock.Now);
					if (alert.Notifications.Count > beforeCount)
					{
						savedNotifications++;
					}
				}

				alert.MarkAsChecked(Clock.Now);
				await _alertRepository.UpdateAsync(alert, autoSave: true);
				await SaveApiAccessAsync(alert.SearchText, startedAt, false, null);
			}
			catch (Exception ex)
			{
				await SaveApiAccessAsync(alert.SearchText, startedAt, true, ex.Message);
				throw;
			}
		}

		return savedNotifications;
	}

	private async Task SaveApiAccessAsync(string searchText, DateTime startedAt, bool hasError, string? errorMessage)
	{
		await _apiAccessLogRepository.InsertAsync(
			new ApiAccessLog(GuidGenerator.Create(), searchText, startedAt, Clock.Now, hasError, errorMessage),
			autoSave: true
		);
	}

	private Guid GetCurrentUserId()
	{
		if (!CurrentUser.Id.HasValue)
		{
			throw new AbpAuthorizationException("Current user is not authenticated.");
		}

		return CurrentUser.Id.Value;
	}
}
