using System.IO;

namespace TlhPlatform.Core.Utility
{
    public interface IDelayFileSystemWatcher
    {
        void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e);
    }
}