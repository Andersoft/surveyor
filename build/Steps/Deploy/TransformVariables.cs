using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Build.Extensions.Docker;
using Build.Extensions.JSON;
using Build.Steps.Build;
using Cake.Common;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Frosting;

namespace Build.Steps.Deploy;

public sealed class TransformVariables : AsyncFrostingTask<BuildContext>
{
  // Tasks can be asynchronous
  public override async Task RunAsync(BuildContext context)
  {
      var options = new TransformVariablesOptions()
      {
        Arguments = context.Arguments(),
        ConfigFile = context.ConfigFile,
        SecretsFile= context.SecretsFile,
        Destination = "./artifacts/settings"
      };

      await context.Transform(options);
    
  }
}

public class TransformVariablesOptions
{
  public IDictionary<string, ICollection<string>> Arguments { get; set; }
  public FileInfo SecretsFile { get; set; }
  public string Destination { get; set; }
  public FileInfo ConfigFile { get; set; }
}