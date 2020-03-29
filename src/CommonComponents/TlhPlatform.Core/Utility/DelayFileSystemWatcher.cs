using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TlhPlatform.Core.Utility
{
    public class DelayFileSystemWatcher : IDelayFileSystemWatcher
    {
        private readonly Timer _mTimer;
        private readonly Int32 _mTimerInterval;
        private readonly FileSystemEventHandler _mFileSystemEventHandler;
        private readonly Dictionary<String, FileSystemEventArgs> _mChangedFiles = new Dictionary<string, FileSystemEventArgs>();
         /// <summary>
         /// 文件监控
         /// </summary>
         /// <param name="path"></param>
         /// <param name="filter"></param>
         /// <param name="watchHandler"></param>
         /// <param name="timerInterval"></param>
        public DelayFileSystemWatcher(string path, string filter, FileSystemEventHandler watchHandler, int timerInterval)
        {
            _mTimer = new Timer(OnTimer, null, Timeout.Infinite, Timeout.Infinite);
            var mFileSystemWatcher = new FileSystemWatcher(path, filter);
            mFileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime; 
            mFileSystemWatcher.Changed += fileSystemWatcher_Changed; 
            mFileSystemWatcher.EnableRaisingEvents = true;
            mFileSystemWatcher.IncludeSubdirectories = true;
            _mFileSystemEventHandler = watchHandler;
            _mTimerInterval = timerInterval;
        }

        public void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (_mChangedFiles)
            {
                if (!_mChangedFiles.ContainsKey(e.Name))
                {
                    _mChangedFiles.Add(e.Name, e);
                }
            }
            _mTimer.Change(_mTimerInterval, Timeout.Infinite);
        }
        /// <summary>
        /// 文件监控时间
        /// </summary>
        /// <param name="state"></param>
        private void OnTimer(object state)
        {
            Dictionary<String, FileSystemEventArgs> tempChangedFiles = new Dictionary<String, FileSystemEventArgs>();

            lock (_mChangedFiles)
            {
                foreach (KeyValuePair<string, FileSystemEventArgs> changedFile in _mChangedFiles)
                {
                    tempChangedFiles.Add(changedFile.Key, changedFile.Value);
                }
                _mChangedFiles.Clear();
            }

            foreach (KeyValuePair<string, FileSystemEventArgs> changedFile in tempChangedFiles)
            {
                _mFileSystemEventHandler(this, changedFile.Value);
            }
        }
    }
}
