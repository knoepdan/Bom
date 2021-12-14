using System;
using System.IO;

namespace Bom.Web.Common
{
    public class FileContentWatcher : IDisposable
    {
        private string _fileContent = "";

        private FileSystemWatcher _fileWatcher;

        public string WatchedFile { get; private set; }

        public string WatchedFileFullPath => Path.Combine(this._fileWatcher.Path, this.WatchedFile);

        public FileContentWatcher(string directory, string fileToWatch)
        {
            if (string.IsNullOrEmpty(fileToWatch))
            {
                throw new ArgumentException("Passed filename may not be null or empty", nameof(fileToWatch));
            }
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException("Passed directory does not exist: " + directory, nameof(directory));
            }
            if (!File.Exists(Path.Combine(directory, fileToWatch)))
            {
                throw new ArgumentException("Passed filename does not exist:  " + fileToWatch, nameof(fileToWatch));
            }

            this.WatchedFile = fileToWatch;
            this._fileWatcher = new FileSystemWatcher(directory);
            this._fileWatcher.Changed += FileWatcherChanged; // register for all type of events as some tool work with rename/create/delete to change file contents
            this._fileWatcher.Renamed += FileWatcherRenamed;
            this._fileWatcher.Deleted += FileWatcherDeleted;
            this._fileWatcher.Created += FileWatcherCreated;
            this._fileWatcher.Error += FileWatcherError;
            this._fileWatcher.EnableRaisingEvents = true;
            this.ReadFile();
        }

        private void FileWatcherError(object sender, ErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Error");
        }

        private void FileWatcherCreated(object sender, FileSystemEventArgs e)
        {
            ReadFile();
        }

        private void FileWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            ReadFile();
        }

        private void FileWatcherRenamed(object sender, RenamedEventArgs e)
        {
            ReadFile();
        }

        private void FileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            ReadFile();
        }

        private void ReadFile(bool retry = true)
        {
            if (!string.IsNullOrEmpty(this.WatchedFile) && File.Exists(WatchedFileFullPath))
            {
                try
                {
                    this._fileContent = File.ReadAllText(WatchedFileFullPath);
                }catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error reading file: " + ex.Message);
                    System.Threading.Thread.Sleep(40);
                    ReadFile(false);
                }
            }
        }

        public string GetContent()
        {
            return this._fileContent;
        }

        public void Dispose()
        {
            if (this._fileWatcher != null)
            {
                this._fileWatcher.Dispose();
            }
            this.WatchedFile = "";
            this._fileContent = "";
        }
    }
}