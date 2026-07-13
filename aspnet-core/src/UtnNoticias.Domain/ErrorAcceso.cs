using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias
{
	public class ErrorAcceso : AuditedAggregateRoot<Guid>
	{
		public EstadoError estado { get; set; }
		public string codigo { get; set; }
		public string mensaje { get; set; }

	}
}
