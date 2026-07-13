using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias
{
	public class Acceso : AuditedAggregateRoot<Guid>
	{
		public DateTime FechaInicio { get; set; }
		public DateTime FechaFinal { get; set; }
		public ErrorAcceso Error { get; set; }
	}
}
