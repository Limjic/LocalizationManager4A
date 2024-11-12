using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace LocalizationManagerTool
{
    public partial class MainWindow : Window
    {
        DataTable dataTable;
        public MainWindow()
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddLanguageWindow();

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string language = dialog.input.Text;
                dataTable.Columns.Add(language);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Import clicked");
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = ""; // Default file name

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                if (filename.Contains(".csv"))
                {
                    OpenCSV(filename);
                }
            }
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .csv");
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            ExportDataGridToXml(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.xml");
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Exporting to .json");
            ExportDataGridToJson(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.json");

        }

        private void ExportCpp_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Exporting to .cpp");

            // TEST ONLY
            // To move where we wanna import

            ImportXmlToDataGrid(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.xml");
            //ImportJsonToDataGrid(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.json");

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
