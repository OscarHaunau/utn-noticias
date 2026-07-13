using System.ComponentModel.DataAnnotations;

namespace UtnNoticias.ReadingLists;

public class CreateReadingListDto
{
	[Required]
	[StringLength(ReadingListConsts.MaxNameLength)]
	public string Name { get; set; } = string.Empty;
}

