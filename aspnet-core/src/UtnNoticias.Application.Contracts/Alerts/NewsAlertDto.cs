using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace UtnNoticias.Alerts;

public class NewsAlertDto : EntityDto<Guid>
{
	public Guid OwnerId { get; set; }
	public string SearchText { get; set; } = string.Empty;
	public bool IsActive { get; set; }
	public DateTime? LastRunTime { get; set; }
	public ICollection<NewsNotificationDto> Notifications { get; set; } = new List<NewsNotificationDto>();
}
