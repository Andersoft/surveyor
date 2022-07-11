using System;
using System.Threading.Tasks;
using Build.Extensions.Docker;
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
    // create values file
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


[TaskName("Create Values Override")]
[IsDependentOn(typeof(TransformVariables))]
public sealed class OverrideHelmChartValues : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {

    // create values file
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