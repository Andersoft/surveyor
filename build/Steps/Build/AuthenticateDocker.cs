using System;
using System.Threading.Tasks;
using Build.Extensions.Docker;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Authenticate Docker")]
[IsDependentOn(typeof(BuildImage))]
public sealed class AuthenticateDocker : AsyncFrostingTask<BuildContext>
{
  public override async Task RunAsync(BuildContext context)
  {

    var options = new DockerAuthenticationOptions
    {
      WorkingDirectory = context.SolutionPath,
      Username = context.Username,
      Password = context.Password
    };

    if (await context.TryDockerAuthenticate(options) is false)
    {
      throw new Exception("Failed to authenticate with Docker");
    }
  }
}