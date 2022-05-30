using System.Threading.Tasks;
using Cake.Core;
using Cake.Git;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.Core.IO;
using Cake.Docker;
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
  public bool Delay { get; set; }
  public string SolutionPath { get; internal set; }

  public BuildContext(ICakeContext context)
    : base(context)
  {
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
    await Cli.Wrap("docker")
      .WithWorkingDirectory(context.SolutionPath)
      .WithArguments(new[]
      { 
        "build",
        "--build-arg project=Surveyor.Presentation",
        "--target test-results",
        "--output type=local,dest=artifacts/test_results",
        "--progress=plain",
        ".",
        "-t surveyor:latest"
      }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();
  }
}

[TaskName("Publish Application")]
[IsDependentOn(typeof(BuildImage))]
public sealed class PublishApplication : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    await Cli.Wrap("docker")
      .WithWorkingDirectory(context.SolutionPath)
      .WithArguments(new[]
      {
        "build",
        "--build-arg project=Surveyor.Presentation",
        ".",
        "-t surveyor:latest"
      }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();
  }
}

[TaskName("Default")]
[IsDependentOn(typeof(PublishApplication))]
public class DefaultTask : FrostingTask
{
}