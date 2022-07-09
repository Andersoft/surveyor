using System;
using System.IO;
using System.Threading.Tasks;
using Build.Extensions.Helm;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Package Helm Chart")]
[IsDependentOn(typeof(AuthenticateDocker))]
public sealed class PackagehHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    var packageFolder = Path.GetFullPath(Path.Combine(context.SolutionPath, "artifacts/helm/"));

    var options = new HelmPackageOptions
    {
      ChartPath = Path.Combine(context.SolutionPath, "helm"),
      AppVersion = context.Version,
      Destination = packageFolder,
      Version = context.Version
    };

    if (await context.TryPackageHelmChartAsync(options) is false)
    {
      throw new Exception("Failed to package helm chart");
    }
  }
}