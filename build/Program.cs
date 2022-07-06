using System;
using Build.Steps.Build;
using Cake.Frosting;

namespace Build;

public static class Program
{
  public static int Main(string[] args)
  {
    Console.WriteLine(string.Join(" ; ", args));

    return new CakeHost()
      .UseContext<BuildContext>()
      .Run(args);
  }
}