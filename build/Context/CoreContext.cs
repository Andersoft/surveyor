using System.Collections.Generic;
using Cake.Core;
using Cake.Frosting;

namespace Build.Context;

public class CoreContext : FrostingContext
{
  public string SolutionPath { get; internal set; }
  public string ProjectName { get; internal set; }

  public CoreContext(ICakeContext context)
    : base(context)
  {
    EntryLibrary = context.Arguments.GetArgument("entry_library");
    ProjectName = context.Arguments.GetArgument("project_name");
    SolutionPath = context.Arguments.GetArgument("solution_path");
    Tags = context.Arguments.GetArguments("tags");
    HelmRepositoryName = context.Arguments.GetArgument("helm_repository_name");
  }

  public string EntryLibrary { get; set; }
  public IEnumerable<string> Tags { get; set; }
  public string HelmRepositoryName { get; set; }
}