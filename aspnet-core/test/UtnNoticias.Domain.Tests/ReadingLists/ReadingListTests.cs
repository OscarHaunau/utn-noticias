using System;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace UtnNoticias.ReadingLists;

public class ReadingListTests
{
	[Fact]
	public void Debe_Rechazar_Creacion_Con_Nombre_Vacio()
	{
		var excepcion = Should.Throw<BusinessException>(() =>
			new ReadingList(Guid.NewGuid(), Guid.NewGuid(), " ")
		);

		excepcion.Code.ShouldBe(UtnNoticiasDomainErrorCodes.ReadingListNameIsRequired);
	}

	[Fact]
	public void Debe_Rechazar_Item_Duplicado_Por_Url()
	{
		var lista = new ReadingList(Guid.NewGuid(), Guid.NewGuid(), "Lecturas");
		lista.AddItem(
			"titulo-1",
			"https://news.example/noticia-1",
			null,
			null,
			null,
			null,
			null
		);

		var excepcion = Should.Throw<BusinessException>(() =>
			lista.AddItem(
				"titulo-2",
				" https://news.example/noticia-1 ",
				null,
				null,
				null,
				null,
				null
			)
		);

		excepcion.Code.ShouldBe(UtnNoticiasDomainErrorCodes.ReadingListItemAlreadyExists);
	}
}
