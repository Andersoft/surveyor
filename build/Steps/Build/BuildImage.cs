using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Docker;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Publish Application")]
[IsDependentOn(typeof(RunTests))]
public sealed class BuildImage : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {

    var options = new DockerBuildOptions
    {
      DockerfileLocation = ".",
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