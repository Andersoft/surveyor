using System.IO;
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
  const string ArgumentSeparator = " ";

  public static async Task<bool> TryDeployHelmChartAsync(this ICakeContext context, DeployHelmChartOptions options)
  {
    context.Log.Information($"helm upgrade -n {options.Namespace} -f {options.ValuesFile} -i {options.Name} {options.Repository}/{options.ChartName}");

    var result = await Cli.Wrap(BinaryName)
      .WithArguments(new[] { "upgrade", $"-n {options.Namespace}", $"-f {options.ValuesFile}", "-i", $"{options.Name}", $"{options.Repository}/{options.ChartName}" }, false)
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

  public static async Task<bool> TryPublishHelmChartAsync(
    this ICakeContext context,
    HelmPublishOptions options)
  {
    var packages = Directory.EnumerateFiles(options.PackageFolder);

    foreach (var package in packages)
    {
      context.Log.Information($"helm cm-push {package} {options.RepositoryName}");

      var result = await Cli.Wrap(BinaryName)
        .WithArguments(new[] { "cm-push", package, options.RepositoryName }, false)
        .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
        .ExecuteBufferedAsync();

      if (result.ExitCode!=0)
      {
        return false;
      }
    }

    return true;
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

    context.Log.Information($"helm package {options.ChartPath} {arguments}");

    var result = await Cli.Wrap(BinaryName)
      .WithWorkingDirectory(options.ChartPath)
      .WithArguments(new[] { "package", options.ChartPath, arguments }, false)
      .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
      .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
      .ExecuteBufferedAsync();

    return result.ExitCode == 0;
  }
}