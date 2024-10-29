using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        // Méthode pour exporter le contenu de DataGrid en format C++
        private void ExportDataGridToCpp(DataGrid dataGrid)
        {
            if (dataGrid.ItemsSource is IEnumerable<object> items)
            {
                string filePath = "DataGridExport.cpp";

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("// Exported DataGrid content in C++ format");
                    writer.WriteLine("#include <vector>");
                    writer.WriteLine("#include <string>");
                    writer.WriteLine();

                    // Écrit les noms des colonnes
                    writer.WriteLine("std::vector<std::string> columns = {");
                    foreach (var column in dataGrid.Columns)
                    {
                        writer.WriteLine($"    \"{column.Header}\",");
                    }
                    writer.WriteLine("};");
                    writer.WriteLine();

                    // Écrit les données des lignes
                    writer.WriteLine("std::vector<std::vector<std::string>> data = {");
                    foreach (var item in items)
                    {
                        writer.Write("    { ");
                        foreach (var column in dataGrid.Columns)
                        {
                            var cellValue = column.OnBindingValueChanged(item)?.ToString() ?? "NULL";
                            writer.Write($"\"{cellValue}\", ");
                        }
                        writer.WriteLine("},");
                    }
                    writer.WriteLine("};");

                    MessageBox.Show($"Exported DataGrid to {filePath}");
                }
            }
            else
            {
                MessageBox.Show("No data to export.");
            }
        }

        private void ImportCppToDataGrid(string filePath, DataGrid dataGrid)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Expressions régulières pour trouver les colonnes et les données
                var columnPattern = @"std::vector<std::string>\s+columns\s*=\s*\{(.+?)\};";
                var dataPattern = @"std::vector<std::vector<std::string>>\s+data\s*=\s*\{(.+?)\};";

                // Extraire les colonnes
                var columnMatch = Regex.Match(string.Join(" ", lines), columnPattern);
                var columnNames = new List<string>();
                if (columnMatch.Success)
                {
                    var columnData = columnMatch.Groups[1].Value;
                    foreach (var columnName in columnData.Split(','))
                    {
                        columnNames.Add(columnName.Trim().Trim('"'));
                    }
                }
                else
                {
                    MessageBox.Show("Failed to match columns in file.");
                }

                // Configurer les colonnes de la DataGrid
                dataGrid.Columns.Clear();
                foreach (var columnName in columnNames)
                {
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = columnName,
                        Binding = new System.Windows.Data.Binding(columnName)
                    });
                }

                // Extraire les lignes de données
                var dataMatch = Regex.Match(string.Join(" ", lines), dataPattern);
                var rowDataList = new List<Dictionary<string, string>>();
                if (dataMatch.Success)
                {
                    var rowsData = dataMatch.Groups[1].Value;
                    var rowMatches = Regex.Matches(rowsData, @"\{(.+?)\}");
                    foreach (Match rowMatch in rowMatches)
                    {
                        var rowData = rowMatch.Groups[1].Value;
                        var rowValues = rowData.Split(',');

                        // Associer chaque valeur à son nom de colonne
                        var rowDict = new Dictionary<string, string>();
                        for (int i = 0; i < columnNames.Count && i < rowValues.Length; i++)
                        {
                            rowDict[columnNames[i]] = rowValues[i].Trim().Trim('"');
                        }
                        rowDataList.Add(rowDict);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to match data rows in file.");
                }

                // Charger les données dans la DataGrid
                dataGrid.ItemsSource = rowDataList;
                MessageBox.Show($"Import completed successfully from file: {filePath}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error processing data: {ex.Message}");
            }
        }

        private void ImportCpp_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "C++ Files (*.cpp)|*.cpp",
                Title = "Select a C++ File to Import"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImportCppToDataGrid(openFileDialog.FileName, dataGrid);
            }
        }
    }

    // Classe d'extension pour récupérer la valeur d'une cellule
    public static class DataGridExtensions
    {
        public static object OnBindingValueChanged(this DataGridColumn column, object dataItem)
        {
            if (column is DataGridBoundColumn boundColumn)
            {
                var binding = boundColumn.Binding as System.Windows.Data.Binding;
                var property = dataItem.GetType().GetProperty(binding?.Path.Path ?? string.Empty);
                return property?.GetValue(dataItem, null);
            }
            return null;
        }
    }
}
