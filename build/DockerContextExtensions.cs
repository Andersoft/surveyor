using System.Linq;
using System.Threading.Tasks;
using Build.Steps.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

namespace Build;


public static class HelmContextExtensions
{
  const string BinaryName = "helm";
  const string ArgumentSeparator = " ";

  public static async Task<bool> TryPublishHelmChartAsync(
    this ICakeContext context, 
    HelmPublishOptions options)
  {

    context.Log.Information($"helm push {options.ChartName} {options.Remote}");

    var result = await Cli.Wrap(BinaryName)
      .WithWorkingDirectory(options.PackageFolder)
      .WithArguments(new[] { "push", options.ChartName, options.Remote }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }

  public static async Task<bool> TryPackageHelmChartAsync(
    this ICakeContext context,
    HelmPackageOptions options)
  {
    string arguments = string.IsNullOrWhiteSpace(options.AppVersion)
      ? string.Empty
      : $"--app-version {options.AppVersion}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.DependencyUpdate)
      ? string.Empty
      : $"--dependency-update {options.DependencyUpdate}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Destination)
      ? string.Empty
      : $"--destination {options.Destination}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Key)
      ? string.Empty
      : $"--key  {options.Key}{ArgumentSeparator}";
    
    arguments += string.IsNullOrWhiteSpace(options.Keyring)
      ? string.Empty
      : $"--keyring  {options.Keyring}{ArgumentSeparator}";
    
    arguments += string.IsNullOrWhiteSpace(options.PassphraseFile)
      ? string.Empty
      : $"--passphrase-file {options.PassphraseFile}{ArgumentSeparator}";
    
    arguments += string.IsNullOrWhiteSpace(options.Sign)
      ? string.Empty
      : $"--sign {options.Sign}{ArgumentSeparator}";
    
    arguments += string.IsNullOrWhiteSpace(options.Version)
      ? string.Empty
      : $"--version {options.Version}{ArgumentSeparator}";

    context.Log.Information($"helm package {arguments}");

    var result = await Cli.Wrap(BinaryName)
      .WithWorkingDirectory(options.ChartPath)
      .WithArguments(new[] { "package", arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}

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

    var tags = string.Join($"{ArgumentSeparator}-t {options.Username}/{options.Repository}:", options.Tags);
    arguments += string.IsNullOrWhiteSpace(tags)
      ? string.Empty
      : $"-t {options.Username}/{options.Repository}:{tags}{ArgumentSeparator}";

    context.Log.Information($"docker build {arguments}");

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
      : $"--username {options.Username}{ArgumentSeparator}";

    arguments += string.IsNullOrWhiteSpace(options.Password)
      ? string.Empty
      : $"--password {options.Password}{ArgumentSeparator}";

    context.Log.Information($"docker login {arguments}");

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
                                                              options.Repository, 
                                                              tag));
    return tasks.All(x => x.Result);
  }

  public static async Task<bool> TryPushDockerImage(
    ICakeContext context,
    string workingDirectory,
    string username,
    string repository,
    string tag)
  {
    string arguments = string.Empty;
    arguments += string.IsNullOrWhiteSpace(username)
      ? string.Empty
      : $"{username}/";

    arguments += string.IsNullOrWhiteSpace(repository)
      ? string.Empty
      : $"{repository}:";

    arguments += string.IsNullOrWhiteSpace(tag)
      ? string.Empty
      : $"{tag}";

    context.Log.Information($"docker push {arguments}");

    var result = await Cli.Wrap(DockerBinaryName)
      .WithWorkingDirectory(workingDirectory)
      .WithArguments(new[] { "push", arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate((info) => context.Log.Information(info)))
      .WithStandardErrorPipe(PipeTarget.ToDelegate((error) => context.Log.Error(error)))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}