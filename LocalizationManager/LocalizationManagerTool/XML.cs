using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public void ExportDataGridToXml(DataGrid dataGrid, string filePath)
        {
            // Ensure DataGrid is bound to a DataTable
            if (dataGrid.ItemsSource is DataView dataView && dataView.Table is DataTable dataTable)
            {
                // Set a name for the DataTable if it does not have one
                if (string.IsNullOrEmpty(dataTable.TableName))
                {
                    dataTable.TableName = "ExportedData";  // Set your desired table name
                }

                // Write the DataTable to an XML file
                dataTable.WriteXml(filePath, XmlWriteMode.WriteSchema);
            }
            else
            {
                MessageBox.Show("DataGrid is not bound to a DataTable.");
            }
        }

        public void ImportXmlToDataGrid(DataGrid dataGrid, string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found.");
                return;
            }

            // Create a new DataTable to hold the XML data
            var dataTable = new DataTable();

            // Read the XML data into the DataTable
            dataTable.ReadXml(filePath);

            // Set the DataGrid's ItemsSource to the DataTable's DefaultView
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
