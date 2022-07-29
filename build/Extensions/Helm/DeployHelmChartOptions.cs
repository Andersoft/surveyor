namespace Build.Extensions.Helm;

public class DeployHelmChartOptions
{
  public string Namespace { get; set; }
  public string Repository { get; set; }
  public string ChartName { get; set; }
  public string ValuesFile { get; set; }
  public string Name { get; set; }
  public string ImageRepository { get; set; }
  public string IngressEnabled { get; set; }
}