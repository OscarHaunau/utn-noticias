using System;
using System.ComponentModel.DataAnnotations;

namespace UtnNoticias.ReadingLists;

public class AddReadingListItemDto
{
	[Required]
	[StringLength(ReadingListConsts.MaxTitleLength)]
	public string Title { get; set; } = string.Empty;

	[Required]
	[StringLength(ReadingListConsts.MaxUrlLength)]
	public string Url { get; set; } = string.Empty;

	[StringLength(ReadingListConsts.MaxAuthorLength)]
	public string? Author { get; set; }

	[StringLength(ReadingListConsts.MaxDescriptionLength)]
	public string? Description { get; set; }

	[StringLength(ReadingListConsts.MaxImageUrlLength)]
	public string? UrlToImage { get; set; }

	public DateTime? PublishedAt { get; set; }

	[StringLength(ReadingListConsts.MaxContentLength)]
	public string? Content { get; set; }
}

