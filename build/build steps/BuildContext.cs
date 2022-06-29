using Cake.Core;
using Cake.Frosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Build.build_steps
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
      Username = context.Arguments.GetArgument("username");
      Password = context.Arguments.GetArgument("password");
      Tags = context.Arguments.GetArguments("tags");
    }

    public string EntryLibrary { get; set; }

    public IEnumerable<string> Tags { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
