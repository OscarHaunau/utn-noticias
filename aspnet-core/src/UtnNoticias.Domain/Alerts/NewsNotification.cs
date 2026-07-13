using System;
using Volo.Abp.Domain.Entities;

namespace UtnNoticias.Alerts;

// Notificacion persistida para mostrar en el area de notificaciones del usuario.
// Libro: p.52: esta clase se persiste con EF Core como tabla de notificaciones.
public class NewsNotification : Entity<Guid>
{
	public Guid AlertId { get; private set; }
	public Guid OwnerId { get; private set; }
	public string Title { get; private set; } = string.Empty;
	public string Url { get; private set; } = string.Empty;
	public DateTime CreatedAt { get; private set; }
	public bool IsRead { get; private set; }

	protected NewsNotification()
	{
	}

	internal NewsNotification(Guid id, Guid alertId, Guid ownerId, string title, string url, DateTime createdAt) : base(id)
	{
		AlertId = alertId;
		OwnerId = ownerId;
		Title = title;
		Url = url;
		CreatedAt = createdAt;
		IsRead = false;
	}
}
