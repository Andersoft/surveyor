using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Git;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.Core.IO;
using CliWrap.Buffered;
using CliWrap;
using System.Text;

public static class Program
{
  public static int Main(string[] args)
  {
    return new CakeHost()
        .UseContext<BuildContext>()
        .Run(args);
  }
}

public class BuildContext : FrostingContext
{
  public string SolutionPath { get; internal set; }
  public string ProjectName { get; internal set; }

  public BuildContext(ICakeContext context)
    : base(context)
  {
    ProjectName = context.Arguments.GetArgument("project_name");
    SolutionPath = context.Arguments.GetArgument("solution_path");
  }
}

[TaskName("Clean Solution")]
public sealed class CleanSolution : FrostingTask<BuildContext>
{
  public override void Run(BuildContext context)
  {
    context.GitClean(context.SolutionPath);
  }
}

[TaskName("Build Image")]
[IsDependentOn(typeof(CleanSolution))]
public sealed class BuildImage : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    var options = new DockerBuildOptions
    {
      DockerfileLocation = ".",
      OutputDirectory = "artifacts/test_results",
      Target = "test-results",
      WorkingDirectory = context.SolutionPath,
      Tags = new[] { "surveyor:latest" },
      BuildArguments = new Dictionary<string, string>
      {
        ["project"] = context.ProjectName
      }
    };

    if (await context.TryBuildDockerfileAsync(options) is false)
    {
      throw new Exception("Docker build failed");
    }
  }
}

[TaskName("Publish Application")]
[IsDependentOn(typeof(BuildImage))]
public sealed class PublishApplication : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {

    var options = new DockerBuildOptions
    {
      DockerfileLocation = ".",
      WorkingDirectory = context.SolutionPath,
      Tags = new[] { "surveyor:latest" },
      BuildArguments = new Dictionary<string, string>
      {
        ["project"] = context.ProjectName
      }
    };

    if (await context.TryBuildDockerfileAsync(options) is false)
    {
      throw new Exception("Docker build failed");
    }
  }
}

[TaskName("Default")]
[IsDependentOn(typeof(PublishApplication))]
public class DefaultTask : FrostingTask
{
}