using System.IO;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.JSON;
using Cake.Common;
using Cake.Frosting;

namespace Build.Steps.Deploy;

[TaskName("Transform Variables")]
public sealed class TransformVariables : AsyncFrostingTask<DeployContext>
{
  public override async Task RunAsync(DeployContext context)
  {
    var options = new TransformVariablesOptions()
    {
      Arguments = context.Arguments(),
      ConfigFile = context.ConfigFile,
      SecretsFile = context.SecretsFile,
      Destination = Path.Combine(context.SolutionPath, "./artifacts/settings")
    };

    await context.Transform(options);
  }
}