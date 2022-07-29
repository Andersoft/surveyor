using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Build.Extensions.Docker;
using Build.Steps.Deploy;
using Cake.Core;
using Cake.Core.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

namespace Build.Extensions.Helm;

public static class HelmContextExtensions
{
  const string BinaryName = "helm";

  public static async Task<bool> TryUpdateHelmRepositoriesAsync(this ICakeContext context){
    context.Log.Information($"helm repo update");

    var result = await Cli.Wrap(BinaryName)
      .WithArguments(new[] { "repo", "update" }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
  
  public static async Task<bool> TryDeployHelmChartAsync(this ICakeContext context, DeployHelmChartOptions options)
  {
    string[] arguments = new[] { "upgrade", $"-n {options.Namespace}", $"-f {options.ValuesFile}", "-i", $"{options.Name}", $"{options.Repository}/{options.ChartName}"};
    context.Log.Information(string.Join(" ", arguments));

    await Cli.Wrap(BinaryName)
      .WithArguments(arguments.Union(new[]{"--dry-run"}), false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    var result = await Cli.Wrap(BinaryName)
      .WithArguments(arguments, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }

  public static async Task<bool> TryAddHelmRepositoryAsync(
    this ICakeContext context,
    HelmRepositoryOptions options)
  {

    context.Log.Information($"helm repo add {options.RepositoryName} {options.RepositoryAddress}");

    var result = await Cli.Wrap(BinaryName)
      .WithArguments(new[] { "repo add", options.RepositoryName, options.RepositoryAddress }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}