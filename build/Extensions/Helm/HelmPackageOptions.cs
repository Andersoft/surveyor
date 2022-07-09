namespace Build;

public class HelmPackageOptions
{
    public string AppVersion { get; set; }
    public string DependencyUpdate { get; set; }
    public string Destination { get; set; }
    public string Key { get; set; }
    public string Keyring { get; set; }
    public string PassphraseFile { get; set; }
    public string Sign { get; set; }
    public string Version { get; set; }
    public string ChartPath { get; set; }
}