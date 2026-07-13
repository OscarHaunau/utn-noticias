using System.ComponentModel.DataAnnotations;

namespace UtnNoticias.Alerts;

public class CreateNewsAlertDto
{
	[Required]
	[StringLength(200)]
	public string SearchText { get; set; } = string.Empty;
}
