using System.Windows;
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
            _vm = (PdfWatcherViewModel)DataContext;
            _vm.Watcher.Changed += Watcher_Changed;

            pdfViewer.Navigate(_vm.FileUri);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (_vm.FInfo != null)
            {
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
                    _vm.Folder = fbd.SelectedPath;
                }
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }
    }
}
