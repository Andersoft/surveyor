using System.Linq;
using Build.Context;
using Cake.Frosting;

namespace Build;

public static class Program
{
  public static int Main(string[] args)
  {
    var isDeployment = args.Any(x => x.Contains("--target=Create Namespace")
                                     || x.Contains("--target=Deploy Helm Chart")
                                     || x.Contains("--target=Transform Variables"));
    var host = new CakeHost();
      
    return (isDeployment 
          ? host.UseContext<DeployContext>() 
          : host.UseContext<BuildContext>())
        .Run(args);
  }
}