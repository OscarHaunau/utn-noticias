using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias.Alerts;

// Alerta simple: guarda el texto que el usuario quiere revisar periodicamente.
// Libro: p.52: EF Core guarda clases C# como entidades en la base de datos.
public class NewsAlert : AuditedAggregateRoot<Guid>
{
	public Guid OwnerId { get; private set; }
	public string SearchText { get; private set; } = string.Empty;
	public bool IsActive { get; private set; }
	public DateTime? LastRunTime { get; private set; }
	public ICollection<NewsNotification> Notifications { get; private set; } = new List<NewsNotification>();

	protected NewsAlert()
	{
	}

	public NewsAlert(Guid id, Guid ownerId, string searchText) : base(id)
	{
		OwnerId = ownerId;
		IsActive = true;
		SetSearchText(searchText);
	}

	public void SetSearchText(string searchText)
	{
		if (string.IsNullOrWhiteSpace(searchText))
		{
			throw new BusinessException(UtnNoticiasDomainErrorCodes.AlertSearchTextIsRequired);
		}

		SearchText = searchText.Trim();
	}

	public void MarkAsChecked(DateTime checkedAt)
	{
		LastRunTime = checkedAt;
	}

		// Regla simple del dominio: una noticia encontrada genera una notificacion.
	// Libro: p.35 habla de ubicar reglas de negocio fuera del controlador.
	public NewsNotification AddNotification(string title, string url, DateTime createdAt)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			title = "Noticia sin titulo";
		}

		if (string.IsNullOrWhiteSpace(url))
		{
			url = "sin-url";
		}

		var normalizedUrl = url.Trim().ToLowerInvariant();
		if (Notifications.Any(x => x.Url.Trim().ToLowerInvariant() == normalizedUrl))
		{
			return Notifications.First(x => x.Url.Trim().ToLowerInvariant() == normalizedUrl);
		}

		var notification = new NewsNotification(Guid.NewGuid(), Id, OwnerId, title.Trim(), url.Trim(), createdAt);
		Notifications.Add(notification);
		return notification;
	}
}
