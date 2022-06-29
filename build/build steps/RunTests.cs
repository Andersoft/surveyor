using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Frosting;

namespace Build.build_steps;

[TaskName("Build Dockerfile")]
[IsDependentOn(typeof(ConfigureDirectories))]
public sealed class RunTests : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
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
        ["project"] = context.EntryLibrary
      }
    };

    if (await context.TryBuildDockerfileAsync(options) is false)
    {
      throw new Exception("Docker build failed");
    }
  }
}