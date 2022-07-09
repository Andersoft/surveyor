namespace Build;

public class HelmPublishOptions
{
    public string PackageFolder { get; set; }
    public string RepositoryName { get; set; }
    public string WorkingDirectory { get; set; }
}