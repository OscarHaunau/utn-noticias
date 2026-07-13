using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UtnNoticias.News
{
	public interface IUtnNoticiasService
	{
		Task<ICollection<NewsDto>> Search(string query);
	}
}
