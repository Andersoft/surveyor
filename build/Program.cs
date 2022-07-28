using System;
using System.Collections.Generic;
using System.Linq;
using Build.Context;
using Cake.Frosting;

namespace Build;

public static class Program
{
  public static int Main(string[] args)
  {
    Console.WriteLine("Arguments");
    Console.WriteLine("-----------------------");
    Console.WriteLine(string.Join(Environment.NewLine, args));
    Console.WriteLine("Environment");
    Console.WriteLine("-----------------------");
    var variables = Environment.GetEnvironmentVariables();
    Console.WriteLine(string.Join(Environment.NewLine, variables.Keys.Cast<string>().Select(x => $"{x}={variables[x]}")));

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