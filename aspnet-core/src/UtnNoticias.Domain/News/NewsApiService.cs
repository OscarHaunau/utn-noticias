using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace UtnNoticias.News
{
	// Libro: p.35 separa la obtencion de datos en una clase de servicio.
	public class NewsApiService : INewsService
	{
		// Libro: p.42 usa async/await para esperar respuestas de red sin bloquear la aplicacion.
		public async Task<ICollection<ArticleDto>> GetNewsAsync(string query)
		{
			var responseList = new List<ArticleDto>();

			// Version simple para el TP: se llama a NewsAPI con HttpClient y se lee el JSON basico.
			// Para no subir claves a GitHub publico, la key se lee desde una variable de entorno.
			var apiKey = Environment.GetEnvironmentVariable("NEWS_API_KEY");
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				// Si no hay key configurada, devolvemos una lista vacia.
				// Asi la app no se rompe y se puede explicar facil en la presentacion.
				return responseList;
			}
			var from = DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM-dd");
			var url = "https://newsapi.org/v2/everything" +
			          $"?q={Uri.EscapeDataString(query)}" +
			          "&sortBy=popularity" +
			          "&language=en" +
			          $"&from={from}" +
			          $"&apiKey={apiKey}";

			using var httpClient = new HttpClient();
			using var response = await httpClient.GetAsync(url);
			var json = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				return responseList;
			}

			using var document = JsonDocument.Parse(json);
			if (!document.RootElement.TryGetProperty("status", out var status) || status.GetString() != "ok")
			{
				return responseList;
			}

			if (!document.RootElement.TryGetProperty("articles", out var articles))
			{
				return responseList;
			}

			foreach (var article in articles.EnumerateArray())
			{
				responseList.Add(new ArticleDto
				{
					Author = GetString(article, "author"),
					Title = GetString(article, "title"),
					Description = GetString(article, "description"),
					Url = GetString(article, "url"),
					UrlToImage = GetString(article, "urlToImage"),
					PublishedAt = GetDate(article, "publishedAt"),
					Content = GetString(article, "content")
				});
			}

			return responseList;
		}

		private static string GetString(JsonElement element, string propertyName)
		{
			return element.TryGetProperty(propertyName, out var property) ? property.GetString() ?? string.Empty : string.Empty;
		}

		private static DateTime? GetDate(JsonElement element, string propertyName)
		{
			if (!element.TryGetProperty(propertyName, out var property))
			{
				return null;
			}

			return DateTime.TryParse(property.GetString(), out var date) ? date : null;
		}
	}
}
