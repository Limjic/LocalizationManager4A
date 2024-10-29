using System.Collections.Generic;
using System.IO;
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
