using System.Collections.Generic;

namespace Build;

public class DockerPushOptions
{
  public string WorkingDirectory { get; set; }
  public string Username { get; set; }
  public string Repository { get; set; }
  public IEnumerable<string> Tags { get; set; }
}