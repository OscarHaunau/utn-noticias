using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias
{
	public class Busqueda : AuditedAggregateRoot<Guid>
	{
		public string Consulta { get; set; }
		public DateTime FechaConsulta { get; set; }


	}
}
