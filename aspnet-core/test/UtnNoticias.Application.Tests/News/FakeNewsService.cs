using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UtnNoticias.News;

// Libro: p.106: en pruebas se pueden usar dependencias simuladas para aislar lo que se prueba.
public class FakeNewsService : INewsService
{
	public Task<ICollection<ArticleDto>> GetNewsAsync(string query)
	{
		ICollection<ArticleDto> articles = new List<ArticleDto>
		{
			new ArticleDto
			{
				Author = "Autor 1",
				Title = $"Noticia de prueba sobre {query}",
				Description = "Descripcion simple para test",
				Url = $"https://news.example/{query}-1",
				UrlToImage = "https://news.example/image.jpg",
				PublishedAt = new DateTime(2026, 7, 1),
				Content = "Contenido de prueba"
			},
			new ArticleDto
			{
				Author = "Autor 2",
				Title = $"Otra noticia sobre {query}",
				Description = "Otra descripcion",
				Url = $"https://news.example/{query}-2",
				UrlToImage = "https://news.example/image2.jpg",
				PublishedAt = new DateTime(2026, 7, 2),
				Content = "Mas contenido"
			}
		};

		return Task.FromResult(articles);
	}
}
