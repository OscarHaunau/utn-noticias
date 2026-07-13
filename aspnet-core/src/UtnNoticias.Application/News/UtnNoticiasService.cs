using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtnNoticias.Monitoring;
using Volo.Abp.Domain.Repositories;

namespace UtnNoticias.News
{
	// Libro: p.35 recomienda separar la logica en servicios para no poner todo en controladores.
	public class UtnNoticiasService : UtnNoticiasAppService, IUtnNoticiasService
	{
		// Libro: p.40 explica inyeccion de dependencias por constructor.
		private readonly INewsService _newsService;
		private readonly IRepository<ApiAccessLog, Guid> _apiAccessLogRepository;

		public UtnNoticiasService(INewsService newsService, IRepository<ApiAccessLog, Guid> apiAccessLogRepository)
		{
			_newsService = newsService;
			_apiAccessLogRepository = apiAccessLogRepository;
		}

		// Libro: p.42 relaciona Task/async con llamadas a base de datos o APIs externas.
		public async Task<ICollection<NewsDto>> Search(string query)
		{
			// Busca en NewsAPI y deja un registro simple para el panel de monitoreo.
			var startedAt = Clock.Now;
			try
			{
				var news = await _newsService.GetNewsAsync(query);
				await SaveApiAccessAsync(query, startedAt, false, null);
				return ObjectMapper.Map<ICollection<ArticleDto>, ICollection<NewsDto>>(news);
			}
			catch (Exception ex)
			{
				await SaveApiAccessAsync(query, startedAt, true, ex.Message);
				throw;
			}
		}

		private async Task SaveApiAccessAsync(string query, DateTime startedAt, bool hasError, string? errorMessage)
		{
			await _apiAccessLogRepository.InsertAsync(
				new ApiAccessLog(GuidGenerator.Create(), query, startedAt, Clock.Now, hasError, errorMessage),
				autoSave: true
			);
		}
	}
}
