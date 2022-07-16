using System.Collections.Generic;
using System.IO;

namespace Build.Extensions.JSON;

public class TransformVariablesOptions
{
  public IDictionary<string, ICollection<string>> Arguments { get; set; }
  public FileInfo SecretsFile { get; set; }
  public string Destination { get; set; }
  public FileInfo ConfigFile { get; set; }
}