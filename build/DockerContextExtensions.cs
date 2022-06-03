using System.Linq;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

public static class DockerContextExtensions
{
  const string DockerBinaryName = "docker";
  const string ArgumentSeparator = " ";
  public static async Task<bool> TryBuildDockerfileAsync(this ICakeContext context, DockerBuildOptions options)
  {
    var buildArgs = string.Join($"{ArgumentSeparator}--build-arg", options.BuildArguments.Select(kvp => $"{kvp.Key}={kvp.Value}"));

    string arguments = string.IsNullOrWhiteSpace(buildArgs)
      ? string.Empty
      : $"--build-arg {buildArgs}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Target)
      ? string.Empty
      : $"--target {options.Target}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.OutputDirectory)
      ? string.Empty
      : $"--output type=local,dest={options.OutputDirectory}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.DockerfileLocation)
      ? string.Empty
      : $"{options.DockerfileLocation}{ArgumentSeparator}";

    arguments += $"-t {string.Join($"{ArgumentSeparator}-t", options.Tags)}";

    var result = await Cli.Wrap(DockerBinaryName)
      .WithWorkingDirectory(options.WorkingDirectory)
      .WithArguments(new[] { "build", arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}