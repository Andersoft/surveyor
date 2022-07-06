using System;
using System.Threading.Tasks;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Publish Image")]
[IsDependentOn(typeof(PublishHelmChart))]
public sealed class PublishImage : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {

    var options = new DockerPushOptions
    {
      WorkingDirectory = context.SolutionPath,
      Tags = context.Tags,
      Repository = context.ProjectName.ToLower(),
      Username = context.Username
    };

    if (await context.TryPushDockerImage(options) is false)
    {
      throw new Exception("Failed to push Docker image");
    }
  }
}

[TaskName("Package Helm Chart")]
[IsDependentOn(typeof(AuthenticateDocker))]
public sealed class PackagehHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {

    var options = new HelmPackageOptions
    {
      ChartPath = context.SolutionPath,
      AppVersion = context.Version,
      Destination = "./artifacts/helm"
    };

    if (await context.TryPackageHelmChartAsync(options) is false)
    {
      throw new Exception("Failed to package helm chart");
    }
  }
}

[TaskName("Package Helm Chart")]
[IsDependentOn(typeof(PackagehHelmChart))]
public sealed class PublishHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    var options = new HelmPublishOptions
    {
      PackageFolder = "./artifacts/helm",
      Remote = context.HelmRepository,
    };

    if (await context.TryPublishHelmChartAsync(options) is false)
    {
      throw new Exception("Failed to package helm chart");
    }
  }
}

public class HelmPublishOptions 
{
  public string PackageFolder { get; set; }
  public string Remote { get; set; }
  public string ChartName { get; set; }
}

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