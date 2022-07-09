using System;
using System.Threading.Tasks;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Publish Helm Chart")]
[IsDependentOn(typeof(AddRemoteRepository))]
public sealed class PublishHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    var options = new HelmPublishOptions
    {
      PackageFolder = "./artifacts/helm",
      RepositoryName = context.HelmRepositoryName
    };

    if (await context.TryPublishHelmChartAsync(options) is false)
    {
      throw new Exception("Failed to package helm chart");
    }
  }
}