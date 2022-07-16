using Build.Context;
using Cake.Frosting;

namespace Build.Steps.Build;

[TaskName("Configure Directories")]
[IsDependentOn(typeof(CleanSolution))]
public sealed class ConfigureDirectories : FrostingTask<BuildContext>
{
  public override void Run(BuildContext context)
  {
    var testDirectory = System.IO.Path.Combine(context.SolutionPath, "artifacts/test_results");

    if (System.IO.Directory.Exists(testDirectory))
    {
      System.IO.Directory.CreateDirectory(testDirectory);
    }
  }
}