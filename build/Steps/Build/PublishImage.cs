using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Docker;
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