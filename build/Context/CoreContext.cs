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
    EntryPoint = context.Arguments.GetArgument("entry_point");
    ProjectName = context.Arguments.GetArgument("project_name");
    SolutionPath = context.Arguments.GetArgument("solution_path");
    Tags = context.Arguments.GetArguments("tags");
    
  }

  public string EntryPoint { get; set; }
  public IEnumerable<string> Tags { get; set; }
}