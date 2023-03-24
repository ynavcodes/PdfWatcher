using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PdfWatcher
{
    public class PdfWatcherViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool SetValue<T>(ref T currentValue, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (currentValue == null || !currentValue.Equals(newValue) || !string.IsNullOrEmpty(propertyName))
            {
                if (newValue == null)
                    return false;

                currentValue = newValue;
                if (propertyName != null)
                    NotifyPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        public PdfWatcherViewModel()
        {
            Folder = AppDomain.CurrentDomain.BaseDirectory;

            Watcher = new FileSystemWatcher
            {
                Path = Folder,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.pdf",
                EnableRaisingEvents = true
            };

            Watcher.Changed += Watcher_Changed;
        }

        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            FilePath = e.FullPath;
        }

        private FileSystemWatcher _watcher;
        public FileSystemWatcher Watcher
        {
            get => _watcher;
            set => SetValue(ref _watcher, value);
        }

        private string _folder;
        public string Folder
        {
            get => _folder;
            set
            {
                if (SetValue(ref _folder, value))
                    ChangeFolder();
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (SetValue(ref _filePath, value))
                    NotifyPropertyChanged(nameof(FileName));
            }
        }

        public string? FileName
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                    return Path.GetFileName(FilePath);
                else
                    return null;
            }
        }

        private void ChangeFolder()
        {
            throw new NotImplementedException();
        }
    }
}
