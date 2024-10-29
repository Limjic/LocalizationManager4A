using System.Windows;

namespace LocalizationManagerTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Import clicked");
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .csv");
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .xml");
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .json");
        }

        private void ExportCpp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .cpp");
        }

        private void ExportCs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .cs");
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
