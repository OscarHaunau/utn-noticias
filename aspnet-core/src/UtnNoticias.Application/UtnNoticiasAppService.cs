using System;
using System.Collections.Generic;
using System.Text;
using UtnNoticias.Localization;
using Volo.Abp.Application.Services;

namespace UtnNoticias;

/* Inherit your application services from this class.
 */
public abstract class UtnNoticiasAppService : ApplicationService
{
	protected UtnNoticiasAppService()
	{
		LocalizationResource = typeof(UtnNoticiasResource);
	}
}
