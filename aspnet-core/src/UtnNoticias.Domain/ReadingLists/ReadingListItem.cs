using System;
using Volo.Abp.Domain.Entities;

namespace UtnNoticias.ReadingLists;

public class ReadingListItem : Entity<Guid>
{
	public Guid ReadingListId { get; private set; }
	public string Title { get; private set; } = string.Empty;
	public string Url { get; private set; } = string.Empty;
	public string? Author { get; private set; }
	public string? Description { get; private set; }
	public string? UrlToImage { get; private set; }
	public DateTime? PublishedAt { get; private set; }
	public string? Content { get; private set; }
	public DateTime AddedAt { get; private set; }

	protected ReadingListItem()
	{
	}

	internal ReadingListItem(
		Guid id,
		Guid readingListId,
		string title,
		string url,
		string? author,
		string? description,
		string? urlToImage,
		DateTime? publishedAt,
		string? content
	) : base(id)
	{
		ReadingListId = readingListId;
		Title = title;
		Url = url;
		Author = author;
		Description = description;
		UrlToImage = urlToImage;
		PublishedAt = publishedAt;
		Content = content;
		AddedAt = DateTime.UtcNow;
	}
}

