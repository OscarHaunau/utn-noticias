using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace UtnNoticias.ReadingLists;

public class ReadingListDto : EntityDto<Guid>
{
	public string Name { get; set; } = string.Empty;
	public Guid OwnerId { get; set; }
	public ICollection<ReadingListItemDto> Items { get; set; } = new List<ReadingListItemDto>();
}

