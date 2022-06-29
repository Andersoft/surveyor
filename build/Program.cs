using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Git;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.Core.IO;
using CliWrap.Buffered;
using CliWrap;
using System.Text;
using Build;

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