using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace LocalizationManagerTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataTable test = new DataTable();
            test.Columns.Add("Name");
            dataGrid.ItemsSource = test.DefaultView;
        }

        private void AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddLanguageWindow();
            
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string language = dialog.input.Text;
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = language;
                dataGrid.Columns.Add(col);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Import clicked");
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
            //MessageBox.Show("Exporting to .json");
            ExportDataGridToJson(dataGrid, "C:\\Users\\Etudiant1\\Desktop\\test\\file.json");
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
