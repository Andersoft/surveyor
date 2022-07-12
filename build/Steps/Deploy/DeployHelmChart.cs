using System;
using System.Threading.Tasks;
using Build.Extensions.Docker;
using Build.Steps.Build;
using Cake.Frosting;

namespace Build.Steps.Deploy;




[TaskName("Deploy Helm Chart")]
[IsDependentOn(typeof(CreateNamespace))]
public sealed class DeployHelmChart : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
    // create values file
    
  }
}