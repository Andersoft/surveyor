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
    ProjectName = context.Arguments.GetArgument("project_name");
    SolutionPath = context.Arguments.GetArgument("solution_path");    
  }
}