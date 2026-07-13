using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias.Monitoring;

// Registro sencillo para saber cuantas veces se llamo a la API y cuanto tardo.
// Libro: p.52: el monitoreo se guarda como entidad simple en base de datos.
public class ApiAccessLog : AuditedAggregateRoot<Guid>
{
	public string SearchText { get; private set; } = string.Empty;
	public DateTime StartedAt { get; private set; }
	public DateTime FinishedAt { get; private set; }
	public int DurationMilliseconds { get; private set; }
	public bool HasError { get; private set; }
	public string? ErrorMessage { get; private set; }

	protected ApiAccessLog()
	{
	}

	public ApiAccessLog(Guid id, string searchText, DateTime startedAt, DateTime finishedAt, bool hasError, string? errorMessage) : base(id)
	{
		SearchText = string.IsNullOrWhiteSpace(searchText) ? "sin busqueda" : searchText.Trim();
		StartedAt = startedAt;
		FinishedAt = finishedAt;
		DurationMilliseconds = Math.Max(0, (int)(finishedAt - startedAt).TotalMilliseconds);
		HasError = hasError;
		ErrorMessage = errorMessage;
	}
}
