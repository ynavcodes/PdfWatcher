using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Application = System.Windows.Application;
using System.IO;

namespace PdfWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PdfWatcherViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new PdfWatcherViewModel();
            _vm = new PdfWatcherViewModel();
            _vm.Watcher.Changed += Watcher_Changed;

            pdfViewer.Navigate(_vm.ViewerUri);
        }

        private void Watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            if (_vm.FilePath != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    pdfViewer.Navigate(_vm.ViewerUri);
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
                    _vm.Folder = fbd.SelectedPath;
                }
            }
        }
    }
}
