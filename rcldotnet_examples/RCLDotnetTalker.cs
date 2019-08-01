using System;
using System.Collections;
using System.Threading;

using rclcs;
using ROS2.Utils;

namespace ConsoleApplication
{
  public class RCLDotnetTalker
  {
    public static void DumpEnvo()
    {
      string str;
      string nl = Environment.NewLine;
      //
      Console.WriteLine();
      Console.WriteLine("-- Environment members --");

      //  Invoke this sample with an arbitrary set of command line arguments.
      Console.WriteLine("CommandLine: {0}", Environment.CommandLine);

      string[] arguments = Environment.GetCommandLineArgs();
      Console.WriteLine("GetCommandLineArgs: {0}", String.Join(", ", arguments));

      //  <-- Keep this information secure! -->
      Console.WriteLine("CurrentDirectory: {0}", Environment.CurrentDirectory);

      Console.WriteLine("ExitCode: {0}", Environment.ExitCode);

      Console.WriteLine("HasShutdownStarted: {0}", Environment.HasShutdownStarted);

      //  <-- Keep this information secure! -->
      Console.WriteLine("MachineName: {0}", Environment.MachineName);

      Console.WriteLine("NewLine: {0}  first line{0}  second line{0}  third line",
      Environment.NewLine);

      Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

      Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);

      //  <-- Keep this information secure! -->
      Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);

      Console.WriteLine("TickCount: {0}", Environment.TickCount);

      //  <-- Keep this information secure! -->
      Console.WriteLine("UserDomainName: {0}", Environment.UserDomainName);

      Console.WriteLine("UserInteractive: {0}", Environment.UserInteractive);

      //  <-- Keep this information secure! -->
      Console.WriteLine("UserName: {0}", Environment.UserName);

      Console.WriteLine("Version: {0}", Environment.Version.ToString());

      Console.WriteLine("WorkingSet: {0}", Environment.WorkingSet);

      //  No example for Exit(exitCode) because doing so would terminate this example.

      //  <-- Keep this information secure! -->
      string query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
      str = Environment.ExpandEnvironmentVariables(query);
      Console.WriteLine("ExpandEnvironmentVariables: {0}  {1}", nl, str);

      Console.WriteLine("GetEnvironmentVariable: {0}  My temporary directory is {1}.", nl,
      Environment.GetEnvironmentVariable("TEMP"));

      Console.WriteLine("GetEnvironmentVariables: ");
      IDictionary environmentVariables = Environment.GetEnvironmentVariables();
      foreach (DictionaryEntry de in environmentVariables)
      {
        Console.WriteLine("  {0} = {1}", de.Key, de.Value);
      }

      Console.WriteLine("GetFolderPath: {0}",
      Environment.GetFolderPath(Environment.SpecialFolder.System));

      string[] drives = Environment.GetLogicalDrives();
      Console.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));
    }

    public static void Main(string[] args)
    {
      DumpEnvo();
      Context ctx = new Context();
      Rclcs.Init(ctx);
      INode node = Rclcs.CreateNode("talker", ctx);
      IPublisher<std_msgs.msg.String> chatter_pub = node.CreatePublisher<std_msgs.msg.String>("chatter");
      std_msgs.msg.String msg = new std_msgs.msg.String();
      int i = 1;

      while (Rclcs.Ok(ctx))
      {
        // adamdbrw - if no at least small sleep is made before
        // the first published message, it doesn't reach subscribers
        // Needs investigation for how to put a valid check here
        // TODO (adam) replace with a proper check
        Thread.Sleep(500);
        msg.Data = "Hello World: " + i;
        i++;
        Console.WriteLine("Publishing: \"" + msg.Data + "\"");
        chatter_pub.Publish(msg);
        Thread.Sleep(500);
      }
      Rclcs.Shutdown(ctx);
    }
  }
}
