using Lib;
using System;
using System.Linq;
using System.Reflection;

namespace WatchFiles
{
    class Program
    {
        static string filesMask;
        static string action;

        static void Main(string[] args)
        {
            Console.WriteLine($"WATCH {Assembly.GetExecutingAssembly().GetName().Version.ToString(2)} (c) 2019 softwaremeisterei");
            Console.WriteLine();

            if (ParseArgs(args))
            {
                new Watcher(filesMask, action).Start();
                RunUntilEscape();
            }
        }

        private static void RunUntilEscape()
        {
            Console.Out.WriteLine($"file watcher started for file mask: {filesMask}");
            Console.Out.WriteLine("press <esc> to exit");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
        }

        private static bool ParseArgs(string[] args)
        {
            var arguments = new Arguments(args);

            if (arguments.Exists("?") ||
                arguments.Exists("h") ||
                arguments.Exists("help"))
            {
                PrintUsage();
                return false;
            }

            filesMask = arguments.Consume("files") ?? "*.*";
            action = arguments.Consume("action");

            var badOptions = arguments.Opts();

            if (badOptions.Any())
            {
                foreach (var opt in badOptions)
                {
                    Console.Error.WriteLine("bad option: {0}", opt);
                }
                return false;
            }

            var tail = arguments.Tail();

            if (tail.Any())
            {
                foreach (var arg in tail)
                {
                    Console.Error.WriteLine("bad argument: {0}", arg);
                }

                return false;
            }

            return true;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: WATCH [-files=<filemask>] [-action=<command>]");
            Console.WriteLine("     <filemask> ... a file mask, e.g. *.txt (defaults to *.*)");
            Console.WriteLine("     <command>  ... command line command to start on any file change, e.g. do-something.bat (defaults to nothing)");
        }
    }
}
