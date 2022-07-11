using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Frosting;

namespace Build.Steps.Build
{
  public class BuildContext : FrostingContext
  {
    public string SolutionPath { get; internal set; }
    public string ProjectName { get; internal set; }

    public BuildContext(ICakeContext context)
      : base(context)
    {
      EntryLibrary = context.Arguments.GetArgument("entry_library");
      ProjectName = context.Arguments.GetArgument("project_name");
      SolutionPath = context.Arguments.GetArgument("solution_path");
      Version = context.Arguments.GetArgument("app_version");
      Username = context.Arguments.GetArgument("username");
      Password = context.Arguments.GetArgument("password");
      Tags = context.Arguments.GetArguments("tags");
      HelmRepositoryName = context.Arguments.GetArgument("helm_repository_name");
      HelmRepositoryAddress = context.Arguments.GetArgument("helm_repository_address");
      ConfigFile = new FileInfo(Path.Combine(SolutionPath, "src", EntryLibrary, "appsettings.json"));
      SecretsFile = new FileInfo(Path.Combine(SolutionPath, "src", EntryLibrary, "appsettings.secrets.json"));
    }

    public string EntryLibrary { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Version { get; set; }
    public string HelmRepositoryName { get; set; }
    public string HelmRepositoryAddress { get; set; }
    public FileInfo SecretsFile { get; set; }
    public FileInfo ConfigFile { get; set; }
  }
}
