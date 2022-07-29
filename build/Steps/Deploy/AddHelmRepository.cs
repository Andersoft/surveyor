using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Docker;
using Build.Extensions.Helm;
using Cake.Frosting;

namespace Build.Steps.Deploy;

[TaskName("Add Helm repository")]
[IsDependentOn(typeof(UpdateHelmRepo))]
public sealed class AddHelmRepository : AsyncFrostingTask<DeployContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(DeployContext context)
  {
    var options = new HelmRepositoryOptions
    {
      RepositoryAddress = context.HelmRepositoryAddress,
      RepositoryName = context.HelmRepositoryName
    };

    if (await context.TryAddHelmRepositoryAsync(options) is false)
    {
      throw new Exception("Failed deploy Helm Chart");
    }
  }
}