using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias
{
	public class RegistroUsuario : AuditedAggregateRoot<Guid>
	{
		public string Correo { get; set; }
		public string Contraseña { get; set; }
		public Usuario usuario { get; set; }


	}
}
