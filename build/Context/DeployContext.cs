using System.IO;
using Cake.Core;

namespace Build.Context;

public class DeployContext : CoreContext
{
  public string ReleaseName { get; set; }
  public string NamespaceLabels { get; set; }
  public string Namespace { get; set; }
  public FileInfo SecretsFile { get; set; }
  public FileInfo ConfigFile { get; set; }

  public DeployContext(ICakeContext context)
    : base(context)
  {
    ConfigFile = new FileInfo(Path.Combine(SolutionPath, "src", EntryLibrary, "appsettings.json"));
    SecretsFile = new FileInfo(Path.Combine(SolutionPath, "src", EntryLibrary, "appsettings.secrets.json"));
  }
}