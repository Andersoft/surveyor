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
  public string HelmRepositoryAddress { get; set; }
  public string HelmRepositoryName { get; set; }
  public string ChartName { get; set; }
  public string ImageRepository { get; set; }
  public string IngressEnabled { get; set; }

  public DeployContext(ICakeContext context)
    : base(context)
  {
    ConfigFile = new FileInfo(Path.Combine(SolutionPath, "src", EntryLibrary, "appsettings.json"));
    SecretsFile = new FileInfo(Path.Combine(SolutionPath, "src", EntryLibrary, "appsettings.secrets.json"));
    NamespaceLabels = context.Arguments.GetArgument("namespace_labels");
    Namespace = context.Arguments.GetArgument("namespace");
    ChartName = context.Arguments.GetArgument("chart_name");
    ReleaseName = context.Arguments.GetArgument("project_name").ToLower();
    HelmRepositoryName = context.Arguments.GetArgument("helm_repository_name");
    HelmRepositoryAddress = context.Arguments.GetArgument("helm_repository_address");
    ImageRepository = context.Arguments.GetArgument("image_repository");
    IngressEnabled = context.Arguments.GetArgument("ingress_enabled");
  }
}