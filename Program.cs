using Lib;
using System;
using System.Linq;

namespace WatchFiles
{
    class Program
    {
        static string filesMask;
        static string action;

        static void Main(string[] args)
        {
            if (ParseArgs(args))
            {
                new Watcher(filesMask, action).Start();
            }

            RunUntilEscape();
        }

        private static void RunUntilEscape()
        {
            Console.Out.WriteLine("<press escape to exit>");
            ConsoleKeyInfo keyPressed;
            do
            {
                keyPressed = Console.ReadKey(true);
            } while (keyPressed.Key != ConsoleKey.Escape);

            Console.Out.WriteLine("bye");
        }

        private static bool ParseArgs(string[] args)
        {
            var arguments = new Arguments(args);

            filesMask = arguments.Consume("files");
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

            string[] tail = arguments.Tail();

            if (tail.Any())
            {
                foreach (var arg in tail)
                {
                    Console.Error.WriteLine("bad arguments: {0}", arg);
                }
                return false;
            }

            return true;
        }
    }
}
