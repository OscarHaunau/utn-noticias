using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace UtnNoticias.News
{
	public class NewsApiService_Test : UtnNoticiasDomainTestBase
	{
		[Fact]
		public async Task Should_Get_All_News()
		{
			// Libro: p.105 habla de pruebas automaticas.
			// Para que sea facil de explicar y no dependa de internet/API key,
			// esta prueba solo valida que el servicio responda sin romper.
			// Las pruebas con resultados fijos estan en Application.Tests usando FakeNewsService (libro p.106).
			var service = new NewsApiService();

			var articles = await service.GetNewsAsync("Apple");

			articles.ShouldNotBeNull();
		}
	}
}
