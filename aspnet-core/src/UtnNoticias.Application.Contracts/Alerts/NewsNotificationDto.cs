using System;
using Volo.Abp.Application.Dtos;

namespace UtnNoticias.Alerts;

public class NewsNotificationDto : EntityDto<Guid>
{
	public Guid AlertId { get; set; }
	public Guid OwnerId { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public bool IsRead { get; set; }
}
