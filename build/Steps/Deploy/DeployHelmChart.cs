using System;
using System.IO;
using System.Threading.Tasks;
using Build.Extensions.Docker;
using Build.Extensions.Helm;
using Build.Extensions.Kubectl;
using Build.Steps.Build;
using Cake.Frosting;

namespace Build.Steps.Deploy;




[TaskName("Deploy Helm Chart")]
[IsDependentOn(typeof(CreateNamespace))]
public sealed class DeployHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    var options = new DeployHelmChartOptions
    {
      ValuesFile = Path.Combine(Path.GetFullPath(context.SolutionPath), "artifacts","settings","override.yaml"),
      ChartName = context.ProjectName.ToLower(),
      Namespace = context.Namespace,
      Repository = context.HelmRepositoryName,
      Name = context.ReleaseName
    };

    ;
    if (await context.TryDeployHelmChartAsync(options) is false)
    {
      throw new Exception("Failed deploy Helm Chart");
    }
  }
}