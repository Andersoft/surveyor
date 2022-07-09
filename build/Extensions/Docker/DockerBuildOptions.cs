using System.Collections.Generic;

namespace Build.Extensions.Docker;

public class DockerBuildOptions
{
    public string WorkingDirectory { get; internal set; }
    public Dictionary<string, string> BuildArguments { get; internal set; }
    public string OutputDirectory { get; internal set; }
    public string Target { get; internal set; }
    public IEnumerable<string> Tags { get; internal set; }
    public string DockerfileLocation { get; internal set; }
    public string Repository { get; set; }
    public string Username { get; set; }
}