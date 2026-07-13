using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UtnNoticias.News
{
	public class UtnNoticiasService_Test : UtnNoticiasApplicationTestBase
	{
		private readonly IUtnNoticiasService _UtnNoticiasService;

		public UtnNoticiasService_Test()
		{
			_UtnNoticiasService = GetRequiredService<IUtnNoticiasService>();
		}

		[Fact]
		public async Task Should_Search_News()
		{
			//Arrange
			var query = "Apple";

			//Act
			var news = await _UtnNoticiasService.Search(query);

			//Assert
			news.ShouldNotBeNull();
			news.Count.ShouldBeGreaterThan(1);
		}
	}
}
