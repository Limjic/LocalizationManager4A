using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;


namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public void ExportDataGridToJson(DataGrid dataGrid, string filePath)
        {
            // List to hold each row as a dictionary
            var rows = new List<Dictionary<string, object>>();

            // Loop through each item in the DataGrid
            foreach (var item in dataGrid.Items)
            {
                var row = new Dictionary<string, object>();

                // Loop through each column in the DataGrid
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    string bindingPath = null;

                    // Handle DataGridBoundColumn
                    if (column is DataGridBoundColumn boundColumn)
                    {
                        // Cast BindingBase to Binding to access Path
                        if (boundColumn.Binding is Binding binding)
                        {
                            bindingPath = binding.Path?.Path;
                        }
                    }
                    // Handle DataGridTemplateColumn (if applicable)
                    else if (column is DataGridTemplateColumn templateColumn)
                    {
                        // For template columns, you might need to set a Tag or use an alternative method
                        bindingPath = templateColumn.Header?.ToString(); // Using Header as a simple identifier here
                    }

                    if (bindingPath == null) continue;

                    // Get the value of the cell
                    var cellValue = item.GetType().GetProperty(bindingPath, BindingFlags.Public | BindingFlags.Instance)?.GetValue(item, null);

                    // Add to dictionary
                    row[bindingPath] = cellValue;
                }

                // Add the row to the list
                rows.Add(row);
            }

            // Serialize the list to JSON and write to file
            string json = JsonConvert.SerializeObject(rows, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
