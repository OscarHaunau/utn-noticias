using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;
using Xunit;

namespace UtnNoticias.ReadingLists;

public class ReadingListAppServiceTests : UtnNoticiasApplicationTestBase
{
	private readonly IReadingListAppService _readingListAppService;
	private readonly IRepository<ReadingList, Guid> _readingListRepository;

	public ReadingListAppServiceTests()
	{
		_readingListAppService = GetRequiredService<IReadingListAppService>();
		_readingListRepository = GetRequiredService<IRepository<ReadingList, Guid>>();
	}

	[Fact]
	public async Task Debe_Crear_Lista_Con_Nombre_Valido()
	{
		var listaCreada = await _readingListAppService.CreateAsync(new CreateReadingListDto
		{
			Name = " Lista de investigacion "
		});

		listaCreada.Id.ShouldNotBe(Guid.Empty);
		listaCreada.Name.ShouldBe("Lista de investigacion");
		listaCreada.Items.ShouldBeEmpty();
	}

	[Fact]
	public async Task Debe_Rechazar_Nombre_Vacio()
	{
		var excepcion = await Should.ThrowAsync<AbpValidationException>(async () =>
		{
			await _readingListAppService.CreateAsync(new CreateReadingListDto
			{
				Name = " "
			});
		});

		excepcion.ValidationErrors.ShouldContain(x => x.MemberNames.Contains(nameof(CreateReadingListDto.Name)));
	}

	[Fact]
	public async Task Debe_Actualizar_Lista_Propia()
	{
		var listaCreada = await CrearListaDeLecturaAsync("Lista inicial");

		var listaActualizada = await _readingListAppService.UpdateAsync(listaCreada.Id, new UpdateReadingListDto
		{
			Name = "Lista actualizada"
		});

		listaActualizada.Id.ShouldBe(listaCreada.Id);
		listaActualizada.Name.ShouldBe("Lista actualizada");
		listaActualizada.OwnerId.ShouldBe(listaCreada.OwnerId);
	}

	[Fact]
	public async Task Debe_Agregar_Noticia_A_Lista()
	{
		var listaCreada = await CrearListaDeLecturaAsync("Lista con noticias");

		var listaConItem = await _readingListAppService.AddItemAsync(listaCreada.Id, new AddReadingListItemDto
		{
			Title = "Actualizacion del mercado de bitcoin",
			Url = "https://news.example/actualizacion-bitcoin",
			Author = "Autor de prueba",
			Description = "Descripcion de prueba",
			UrlToImage = "https://news.example/imagen.jpg",
			PublishedAt = new DateTime(2026, 5, 27, 12, 0, 0),
			Content = "Contenido de prueba"
		});

		listaConItem.Items.Count.ShouldBe(1);
		var item = listaConItem.Items.Single();
		item.Title.ShouldBe("Actualizacion del mercado de bitcoin");
		item.Url.ShouldBe("https://news.example/actualizacion-bitcoin");
		item.Author.ShouldBe("Autor de prueba");
	}

	[Fact]
	public async Task Debe_Rechazar_Noticia_Duplicada_Por_Url()
	{
		var listaCreada = await CrearListaDeLecturaAsync("Lista con duplicados");

		await _readingListAppService.AddItemAsync(listaCreada.Id, new AddReadingListItemDto
		{
			Title = "Titulo de noticia",
			Url = "https://news.example/noticia-1",
			Author = "Autor"
		});

		var excepcion = await Should.ThrowAsync<BusinessException>(async () =>
		{
			await _readingListAppService.AddItemAsync(listaCreada.Id, new AddReadingListItemDto
			{
				Title = "Otro titulo",
				Url = " https://news.example/noticia-1 "
			});
		});

		excepcion.Code.ShouldBe(UtnNoticiasDomainErrorCodes.ReadingListItemAlreadyExists);
	}

	[Fact]
	public async Task Debe_Eliminar_Lista()
	{
		var listaCreada = await CrearListaDeLecturaAsync("Lista a eliminar");
		await _readingListAppService.AddItemAsync(listaCreada.Id, new AddReadingListItemDto
		{
			Title = "Item a eliminar",
			Url = "https://news.example/item-a-eliminar"
		});

		await _readingListAppService.DeleteAsync(listaCreada.Id);

		await Should.ThrowAsync<EntityNotFoundException>(async () =>
		{
			await _readingListAppService.GetAsync(listaCreada.Id);
		});
	}

	[Fact]
	public async Task Debe_Rechazar_Operaciones_Sobre_Lista_Ajena()
	{
		var listaAjenaId = Guid.NewGuid();

		var listaAjena = new ReadingList(
			listaAjenaId,
			Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
			"Lista ajena"
		);

		await _readingListRepository.InsertAsync(listaAjena, autoSave: true);

		await Should.ThrowAsync<EntityNotFoundException>(async () =>
		{
			await _readingListAppService.UpdateAsync(listaAjenaId, new UpdateReadingListDto
			{
				Name = "Intento de actualizacion"
			});
		});

		await Should.ThrowAsync<EntityNotFoundException>(async () =>
		{
			await _readingListAppService.AddItemAsync(listaAjenaId, new AddReadingListItemDto
			{
				Title = "Item bloqueado",
				Url = "https://news.example/bloqueado"
			});
		});
	}

	[Fact]
	public async Task Debe_Listar_Solo_Listas_Del_Usuario_Actual()
	{
		await _readingListAppService.CreateAsync(new CreateReadingListDto
		{
			Name = "Lista propia"
		});

		var listaAjena = new ReadingList(
			Guid.NewGuid(),
			Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
			"Lista ajena para consulta"
		);
		await _readingListRepository.InsertAsync(listaAjena, autoSave: true);

		var listasPropias = await _readingListAppService.GetListAsync();

		listasPropias.Any(x => x.OwnerId == Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")).ShouldBeFalse();
	}

	private Task<ReadingListDto> CrearListaDeLecturaAsync(string nombre)
	{
		return _readingListAppService.CreateAsync(new CreateReadingListDto
		{
			Name = nombre
		});
	}
}
