using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias.ReadingLists;

public class ReadingList : AuditedAggregateRoot<Guid>
{
	public string Name { get; private set; } = string.Empty;
	public Guid OwnerId { get; private set; }
	public ICollection<ReadingListItem> Items { get; private set; } = new List<ReadingListItem>();

	protected ReadingList()
	{
	}

	public ReadingList(Guid id, Guid ownerId, string name) : base(id)
	{
		OwnerId = ownerId;
		SetName(name);
	}

	public void SetName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new BusinessException(UtnNoticiasDomainErrorCodes.ReadingListNameIsRequired);
		}

		Name = name.Trim();
	}

	public ReadingListItem AddItem(
		string title,
		string url,
		string? author,
		string? description,
		string? urlToImage,
		DateTime? publishedAt,
		string? content
	)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			throw new BusinessException(UtnNoticiasDomainErrorCodes.ReadingListItemTitleIsRequired);
		}

		if (string.IsNullOrWhiteSpace(url))
		{
			throw new BusinessException(UtnNoticiasDomainErrorCodes.ReadingListItemUrlIsRequired);
		}

		var normalizedUrl = NormalizeUrl(url);
		var hasDuplicatedUrl = Items.Any(x => NormalizeUrl(x.Url) == normalizedUrl);
		if (hasDuplicatedUrl)
		{
			throw new BusinessException(UtnNoticiasDomainErrorCodes.ReadingListItemAlreadyExists);
		}

		var item = new ReadingListItem(
			Guid.NewGuid(),
			Id,
			title.Trim(),
			url.Trim(),
			author?.Trim(),
			description?.Trim(),
			urlToImage?.Trim(),
			publishedAt,
			content?.Trim()
		);

		Items.Add(item);
		return item;
	}

	private static string NormalizeUrl(string url)
	{
		return url.Trim().ToLowerInvariant();
	}
}

