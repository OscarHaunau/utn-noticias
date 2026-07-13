using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace UtnNoticias
{
	public class Alerta : AuditedAggregateRoot<Guid>
	{
		public string Tema { get; set; }	
		public EstadoAlerta Estado { get; set; }
		public Busqueda busqueda { get; set; }
		public ListaContenedor listacontenedor { get; set; }
		

	}
}
