using System.Data;
using System.Windows;

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
            dataTable = new DataTable();
            dataTable.Columns.Add("Name");

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                if (filename.Contains(".csv"))
                {
                    OpenCSV(filename);
                }
                else if(filename.Contains(".xml"))
                {
                    ImportXmlToDataGrid(dataGrid, filename);
                }
                else if(filename.Contains(".json"))
                {
                    ImportJsonToDataGrid(dataGrid, filename);
                }
            }
        }

        private bool GetExportFileName(out string filename, string extension)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "file" + extension;
            bool? result = dialog.ShowDialog();
            filename = "";
            if (result == true)
            {
                filename = dialog.FileName;
                if (!filename.Contains(extension))
                {
                    filename += extension;
                }
                return true;
            }

            return false;
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting to .csv");

        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            GetExportFileName(out filename, ".xml");
            //ExportDataGridToXml(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.xml");
            ExportDataGridToXml(dataGrid, filename);
            
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Exporting to .json");
            ExportDataGridToJson(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.json");

        }

        private void ExportCpp_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Exporting to .cpp");
            ExportDataGridToCpp(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.cpp", "C:\\Users\\Etudiant1\\Desktop\\test\\file.h");


            // TEST ONLY
            // To move where we wanna import

            //ImportXmlToDataGrid(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.xml");
            //ImportJsonToDataGrid(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.json");

        }

        private void ExportCs_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
