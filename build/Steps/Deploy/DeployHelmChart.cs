using System;
using System.IO;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Helm;
using Build.Extensions.Kubectl;
using Cake.Frosting;

namespace Build.Steps.Deploy;

[TaskName("Deploy Helm Chart")]
[IsDependentOn(typeof(UpdateHelmRepo))]
public sealed class DeployHelmChart : AsyncFrostingTask<DeployContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(DeployContext context)
  {
    var options = new DeployHelmChartOptions
    {
      ValuesFile = Path.Combine(Path.GetFullPath(context.SolutionPath), "artifacts", "settings", "override.yaml"),
      ChartName = context.ChartName,
      Namespace = context.Namespace,
      Repository = context.HelmRepositoryName,
      Name = context.ReleaseName,
      ImageRepository = context.ImageRepository,
      IngressEnabled = context.IngressEnabled
    };

    if (await context.TryDeployHelmChartAsync(options) is false)
    {
      throw new Exception("Failed deploy Helm Chart");
    }
  }
}
