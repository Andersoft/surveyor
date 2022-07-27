using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Helm;
using Cake.Frosting;

namespace Build.Steps.Deploy;

[TaskName("Update Helm Repo")]
[IsDependentOn(typeof(CreateNamespace))]
public sealed class UpdateHelmRepo : AsyncFrostingTask<DeployContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(DeployContext context)
  {
    if (await context.TryUpdateHelmRepositoriesAsync() is false)
    {
      throw new Exception("Failed to update Helm repos");
    }
  }
}