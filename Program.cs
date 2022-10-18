using System;
using System.IO;
using System.Threading;

namespace wavfix
{
  class Program
  {
    private static void Main(string[] args)
    {
      for(var i = 0; i < args.Length; i++)
      {
        Console.WriteLine("Processing file {0} out of {1}...", i + 1, args.Length);

        var path = args[i];
        var fileName = Path.GetFileName(path);
        var outPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        if(File.Exists(path) && path.ToLower().EndsWith(".wav"))
        {
          Console.WriteLine("Inspecting {0}...", fileName);

          var format = new byte[2];
          var expectedFormat = new byte[] { 0x01, 0x00 };

          using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
          {
            fs.Position = 20;
            fs.Read(format, 0, expectedFormat.Length);
          }

          if(format[0].Equals(expectedFormat[0]) && format[1].Equals(expectedFormat[1]))
          {
            Console.WriteLine("This file is fine.");

            continue;
          }
          else 
          {
            Console.WriteLine("This file needs fixing! Getting to work...");
          }

          Console.WriteLine("Copying the file...");

          File.Copy(path, outPath, true);

          using(var fs = new FileStream(outPath, FileMode.Open, FileAccess.Write))
          {
            fs.Position = 20;
            fs.Write(expectedFormat, 0, expectedFormat.Length);
          }

          Console.WriteLine("Fixed this file, moving to the next one...");
        }
      }

      Console.WriteLine("All done!");
      Console.ReadKey();
    }
  }
}
