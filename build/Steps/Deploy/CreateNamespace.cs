using System;
using System.Threading.Tasks;
using Build.Extensions.Docker;
using Build.Steps.Build;
using Cake.Frosting;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Build.Steps.Deploy;

[TaskName("Create Namespace")]
[IsDependentOn(typeof(TransformVariables))]
public sealed class CreateNamespace : AsyncFrostingTask<BuildContext>
{
    // Tasks can be asynchronous
    public override async Task RunAsync(BuildContext context)
    {


    var options = new DockerPushOptions
        {
            WorkingDirectory = context.SolutionPath,
            Tags = context.Tags,
            Repository = context.ProjectName.ToLower(),
            Username = context.Username
        };

        if (await context.TryPushDockerImage(options) is false)
        {
            throw new Exception("Failed to push Docker image");
        }
    }
}

public class AppSettings
{
  public string Config { get; set; }
  public string Secrets { get; set; }
}