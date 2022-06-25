using System.Collections.Generic;
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

    var tags = string.Join($"{ArgumentSeparator}-t", options.Tags);
    arguments += string.IsNullOrWhiteSpace(tags)
      ? string.Empty
      : $"-t {tags}{ArgumentSeparator}";

    var result = await Cli.Wrap(DockerBinaryName)
      .WithWorkingDirectory(options.WorkingDirectory)
      .WithArguments(new[] { "build", arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }

  public static async Task<bool> TryDockerAuthenticate(
    this ICakeContext context,
    DockerAuthenticationOptions options)
  {
    string arguments = string.Empty;
    arguments += string.IsNullOrWhiteSpace(options.Username)
      ? string.Empty
      : $"--Username {options.Username}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Password)
      ? string.Empty
      : $"--Password {options.Password}{ArgumentSeparator}";

    var result = await Cli.Wrap(DockerBinaryName)
      .WithWorkingDirectory(options.WorkingDirectory)
      .WithArguments(new[] { "login", arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();
    
    return result.ExitCode == 0;

  }

  public static async Task<bool> TryPushDockerImage(
    this ICakeContext context,
    DockerPushOptions options)
  {

    var tasks = options.Tags.Select(tag => TryPushDockerImage(context, 
                                                              options.WorkingDirectory, 
                                                              options.Username,
                                                              options.Repository, tag));
    return tasks.All(x => x.Result);
  }

  public static async Task<bool> TryPushDockerImage(
    ICakeContext context,
    string workingDirectory,
    string username,
    string repository,
    string tag)
  {
    var result = await Cli.Wrap(DockerBinaryName)
      .WithWorkingDirectory(workingDirectory)
      .WithArguments(new[] { "push", $"{username}/{repository}:{tag}" }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}

public class DockerAuthenticationOptions
{
  public string Username { get; set; }
  public string Password { get; set; }
  public string WorkingDirectory { get; set; }
}

public class DockerPushOptions
{
  public string WorkingDirectory { get; set; }
  public string Username { get; set; }
  public string Repository { get; set; }
  public IEnumerable<string> Tags { get; set; }
}