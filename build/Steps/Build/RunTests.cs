using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Docker;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Build Dockerfile")]
[IsDependentOn(typeof(CleanSolution))]
public sealed class RunTests : AsyncFrostingTask<BuildContext>
{
  public override async Task RunAsync(BuildContext context)
  {
    var options = new DockerBuildOptions
    {
      DockerfileLocation = ".",
      OutputDirectory = "artifacts/test_results/",
      Target = "test-results",
      WorkingDirectory = context.SolutionPath,
      Tags = context.Tags,
      Username = context.Username,
      Repository = context.ProjectName.ToLower(),
      BuildArguments = new Dictionary<string, string>
      {
        ["project"] = context.EntryPoint
      }
    };

    if (await context.TryBuildDockerfileAsync(options) is false)
    {
      throw new Exception("Docker build failed");
    }
  }
}