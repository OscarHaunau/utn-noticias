namespace UtnNoticias.Monitoring;

public class ApiMonitoringDto
{
	public int TotalAccesses { get; set; }
	public int TotalErrors { get; set; }
	public double AverageMilliseconds { get; set; }
}
