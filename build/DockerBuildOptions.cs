using System.Collections.Generic;

public class DockerBuildOptions
{
  public string WorkingDirectory { get; internal set; }
  public Dictionary<string, string> BuildArguments { get; internal set; }
  public string OutputDirectory { get; internal set; }
  public string Target { get; internal set; }
  public string[] Tags { get; internal set; }
  public string DockerfileLocation { get; internal set; }
}