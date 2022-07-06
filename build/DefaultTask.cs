using Build.Steps.Build;
using Cake.Frosting;

namespace Build;

[TaskName("Default")]
[IsDependentOn(typeof(PublishImage))]

public class DefaultTask : FrostingTask
{
}