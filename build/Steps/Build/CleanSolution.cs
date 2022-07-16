using Build.Context;
using Cake.Frosting;
using Cake.Git;

namespace Build.Steps.Build;

[TaskName("Clean Solution")]
public sealed class CleanSolution : FrostingTask<BuildContext>
{
  public override void Run(BuildContext context)
  {
    context.GitClean(context.SolutionPath);

    var testDirectory = System.IO.Path.Combine(context.SolutionPath, "artifacts/test_results");
    if (!System.IO.Directory.Exists(testDirectory))
    {
      System.IO.Directory.CreateDirectory(testDirectory);
    }
  }
}