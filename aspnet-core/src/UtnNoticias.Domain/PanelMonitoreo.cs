using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias
{
	public class PanelMonitoreo : AuditedAggregateRoot<Guid>
	{
		public int CantidadErrores { get; set; }
		public List<Acceso> Accesos { get; set; }

	}
}
