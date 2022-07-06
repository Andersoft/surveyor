using System;
using System.Threading.Tasks;
using Cake.Frosting;

namespace Build.Steps.Build;

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