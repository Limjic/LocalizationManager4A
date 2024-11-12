using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public void ExportDataGridToJson(DataGrid dataGrid, string filePath)
        {
            var rows = new List<Dictionary<string, object>>();

            // Loop through each item in the DataGrid
            foreach (var item in dataGrid.Items)
            {
                // Skip any items that are not DataRowView (such as empty placeholders)
                if (item is not DataRowView rowView) continue;

                var row = new Dictionary<string, object>();

                // Loop through each column in the DataRowView
                foreach (DataColumn column in rowView.Row.Table.Columns)
                {
                    // Get the column name and its corresponding value
                    string columnName = column.ColumnName;
                    object cellValue = rowView.Row[columnName];

                    // Add to the row dictionary
                    row[columnName] = cellValue;
                }

                // Add the row to the rows list
                rows.Add(row);
            }

            // Serialize the list to JSON and write to file
            string json = JsonConvert.SerializeObject(rows, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void ImportJsonToDataGrid(DataGrid dataGrid, string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found.");
                return;
            }

            // Read JSON file
            string json = File.ReadAllText(filePath);

            // Parse JSON into a list of dictionaries (each dictionary represents a row)
            var rows = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

            // Create a DataTable to hold the data
            var dataTable = new DataTable();

            // If there is data, set up columns and rows in the DataTable
            if (rows != null && rows.Count > 0)
            {
                // Create columns in the DataTable based on the JSON keys
                foreach (string columnName in rows[0].Keys)
                {
                    dataTable.Columns.Add(columnName);
                }

                // Add rows to the DataTable
                foreach (var row in rows)
                {
                    var dataRow = dataTable.NewRow();
                    foreach (var kvp in row)
                    {
                        dataRow[kvp.Key] = kvp.Value ?? DBNull.Value; // Use DBNull for null values
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            // Set the DataGrid's ItemsSource to the DataTable's DefaultView
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
