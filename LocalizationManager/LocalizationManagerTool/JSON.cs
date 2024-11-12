using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
