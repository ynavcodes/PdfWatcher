using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using System;

namespace PdfWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PdfWatcherViewModel _vm;
        FileSystemWatcher _watcher { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new PdfWatcherViewModel();
            _vm = (PdfWatcherViewModel)DataContext;

            _watcher = new FileSystemWatcher
            {
                Path = _vm.WatcherFolder,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.pdf",
                EnableRaisingEvents = true
            };

            _watcher.Changed += Watcher_Changed;

            pdfViewer.Navigate(_vm.FileUri);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            _vm.FInfo = new FileInfo(e.FullPath);

            if (_vm.FInfo != null)
            {
                _vm.ShowNoPdfText = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    pdfViewer.Navigate(_vm.FileUri);
                });
            }
        }

        private void btChangeFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath) && Directory.Exists(fbd.SelectedPath))
                {
                    pdfViewer.Navigate(new Uri("about:blank"));
                    _vm.ShowNoPdfText = true;

                    SetAndSaveFolder(fbd.SelectedPath);
                    _watcher.Path = _vm.Folder;
                }
            }
        }

        private void SetAndSaveFolder(string path)
        {
            _vm.Folder = path;
            UserSettings.Default.Folder = _vm.Folder;
            UserSettings.Default.Save();
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", _vm.Folder);
        }
    }
}
