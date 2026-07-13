using System;
using Volo.Abp.Application.Dtos;

namespace UtnNoticias.ReadingLists;

public class ReadingListItemDto : EntityDto<Guid>
{
	public string Title { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public string? Author { get; set; }
	public string? Description { get; set; }
	public string? UrlToImage { get; set; }
	public DateTime? PublishedAt { get; set; }
	public string? Content { get; set; }
	public DateTime AddedAt { get; set; }
}

