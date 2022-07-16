using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Docker;
using Build.Extensions.Helm;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Add Helm repository")]
[IsDependentOn(typeof(PackagehHelmChart))]
public sealed class AddRemoteRepository : AsyncFrostingTask<BuildContext>
{
  public override async Task RunAsync(BuildContext context)
  {
    var options = new HelmRepositoryOptions()
    {
      RepositoryAddress= context.HelmRepositoryAddress,
      RepositoryName = context.HelmRepositoryName
    };

    if (await context.TryAddHelmRepositoryAsync(options) is false)
    {
      throw new Exception("Failed to add helm repository");
    }
  }
}