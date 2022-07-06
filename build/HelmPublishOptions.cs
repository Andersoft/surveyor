namespace Build;

public class HelmPublishOptions
{
    public string PackageFolder { get; set; }
    public string Remote { get; set; }
    public string ChartName { get; set; }
}