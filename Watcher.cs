using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace WatchFiles
{
    internal class Watcher
    {
        private readonly string directory;
        private readonly string filesMask;
        private readonly string action;

        private BehaviorSubject<FileSystemEventArgs> fileChangedSubject = new BehaviorSubject<FileSystemEventArgs>(null);

        public Watcher(string filesMask, string action)
        {
            this.filesMask = filesMask;
            this.action = action;
            this.directory = Directory.GetCurrentDirectory();
        }

        public void Start()
        {
            var watcher = new FileSystemWatcher(directory, filesMask);
            watcher.Changed += OnFileChanged;
            watcher.EnableRaisingEvents = true;

            fileChangedSubject
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(ev =>
                {
                    if (ev != null)
                    {
                        StartAction(ev);
                    }
                });
        }

        private void OnFileChanged(object sender, FileSystemEventArgs ev)
        {
            fileChangedSubject.OnNext(ev);
        }

        private void StartAction(FileSystemEventArgs ev)
        {
            Console.WriteLine("file change detected: {0}", ev.Name);

            if (action != null)
            {
                var command = action.Split(' ').First();
                var arguments = string.Join(" ", action.Split(' ').Skip(1));
                Console.WriteLine($"starting command: {command} with arguments: {arguments}");
                var startInfo = new ProcessStartInfo(command, arguments)
                {
                    UseShellExecute = false
                };
                Process.Start(startInfo);
            }
        }

    }
}