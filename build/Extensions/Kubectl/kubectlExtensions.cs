using System.Threading.Tasks;
using Build.Steps.Deploy;
using Cake.Core;
using Cake.Core.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

namespace Build.Extensions.Kubectl
{
  public static class KubectlExtensions
  {
    public const string BinaryName = "kubectl";

    public async static Task<bool> CreateNamespace(
      this ICakeContext context,
      NamespaceOptions options)
    {
      context.Log.Information($"{BinaryName} create namespace {options.Name}");

      var creation = await (Cli.Wrap(BinaryName).WithArguments(new[] { "create", "namespace", options.Name, "--dry-run=client -o yaml" }, false) | 
                            Cli.Wrap(BinaryName).WithArguments(new[] { "apply -f -" }, false))
        .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
        .ExecuteBufferedAsync();

      context.Log.Information($"{BinaryName} label namespace {(options.Overwrite ? "--overwrite " : string.Empty)} {options.Name} {options.Value}");

      var label = await Cli.Wrap(BinaryName)
        .WithArguments(new[]
        {
          "label",
          "namespace",
          options.Overwrite ? "--overwrite" : string.Empty,
          options.Name,
          options.Value
        }, false)
        .WithStandardOutputPipe(PipeTarget.ToDelegate(context.Log.Information))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(context.Log.Error))
        .ExecuteBufferedAsync();

      return creation.ExitCode + label.ExitCode == 0;

    }
  }
}
