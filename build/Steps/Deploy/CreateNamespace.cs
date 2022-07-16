using System;
using System.Threading.Tasks;
using Build.Extensions.Helm;
using Build.Extensions.Kubectl;
using Build.Steps.Build;
using Cake.Frosting;

namespace Build.Steps.Deploy;

[TaskName("Create Namespace")]
[IsDependentOn(typeof(TransformVariables))]
public sealed class CreateNamespace : AsyncFrostingTask<BuildContext>
{
    // Tasks can be asynchronous
    public override async Task RunAsync(BuildContext context)
    {
      var options = new NamespaceOptions
      {
        Name = "devops",
        Overwrite = true,
        Value = "istio-injection=enable"
      };

      if (await context.CreateNamespace(options) is false)
      {
        throw new Exception("Failed create namespace");
      }
  }
}