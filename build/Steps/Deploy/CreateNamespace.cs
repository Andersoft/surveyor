using System;
using System.Threading.Tasks;
using Build.Context;
using Build.Extensions.Helm;
using Build.Extensions.Kubectl;
using Cake.Frosting;

namespace Build.Steps.Deploy;

[TaskName("Create Namespace")]
[IsDependentOn(typeof(TransformVariables))]
public sealed class CreateNamespace : AsyncFrostingTask<DeployContext>
{
  public override async Task RunAsync(DeployContext context)
  {
    var options = new NamespaceOptions
    {
      Name = context.Namespace,
      Overwrite = true,
      Value = context.NamespaceLabels
    };

    if (await context.CreateNamespace(options) is false)
    {
      throw new Exception("Failed create namespace");
    }
  }
}