using System.ComponentModel.DataAnnotations;

namespace UtnNoticias.ReadingLists;

public class UpdateReadingListDto
{
	[Required]
	[StringLength(ReadingListConsts.MaxNameLength)]
	public string Name { get; set; } = string.Empty;
}

