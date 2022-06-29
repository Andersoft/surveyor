using Build.build_steps;
using Cake.Frosting;

[TaskName("Default")]
[IsDependentOn(typeof(PublishImage))]

public class DefaultTask : FrostingTask
{
}