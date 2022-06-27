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
    Console.WriteLine(string.Join(" ; ", args));
    
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
    EntryLibrary = context.Arguments.GetArgument("entry_library");
    ProjectName = context.Arguments.GetArgument("project_name");
    SolutionPath = context.Arguments.GetArgument("solution_path");
    Username = context.Arguments.GetArgument("username");
    Password = context.Arguments.GetArgument("password");
    Tags = context.Arguments.GetArguments("tags");
  }

  public string EntryLibrary { get; set; }

  public IEnumerable<string> Tags { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }
}

[TaskName("Clean Solution")]
public sealed class CleanSolution : FrostingTask<BuildContext>
{
  public override void Run(BuildContext context)
  {
    context.GitClean(context.SolutionPath);
  }
}

[TaskName("Build Dockerfile")]
[IsDependentOn(typeof(CleanSolution))]
public sealed class RunTests : AsyncFrostingTask<BuildContext>
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

[TaskName("Authenticate Docker")]
[IsDependentOn(typeof(BuildImage))]
public sealed class AuthenticateDocker : AsyncFrostingTask<BuildContext>
{
  public override async Task RunAsync(BuildContext context)
  {

    var options = new DockerAuthenticationOptions
    {
      WorkingDirectory = context.SolutionPath,
      Username = context.Username,
      Password = context.Password
      };

    if (await context.TryDockerAuthenticate(options) is false)
    {
      throw new Exception("Failed to authenticate with Docker");
    }
  }
}

[TaskName("Publish Image")]
[IsDependentOn(typeof(AuthenticateDocker))]
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

[TaskName("Default")]
[IsDependentOn(typeof(PublishImage))]
public class DefaultTask : FrostingTask
{
}