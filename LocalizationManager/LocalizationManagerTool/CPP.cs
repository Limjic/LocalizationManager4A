using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public void ExportDataGridToCpp(DataGrid dataGrid, string cppFilePath, string hFilePath)
        {
            // Ensure DataGrid is bound to a DataTable
            if (dataGrid.ItemsSource is DataView dataView && dataView.Table is DataTable dataTable)
            {
                // Ensure the DataTable has the correct columns
                EnsureColumnsExist(dataTable);

                // Open the .h file for writing
                using (StreamWriter hFile = new StreamWriter(hFilePath))
                {
                    hFile.WriteLine("#include <map>");
                    hFile.WriteLine("using namespace std;");
                    hFile.WriteLine();
                    hFile.WriteLine("extern map<string, string> dataMap;");
                }

                // Open the .cpp file for writing
                using (StreamWriter cppFile = new StreamWriter(cppFilePath))
                {
                    cppFile.WriteLine("#include \"YourHeader.h\"");
                    cppFile.WriteLine("map<string, string> dataMap = {");

                    // Loop through the DataTable and write data to the map initialization in .cpp
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Check that columns "Name" and "Value" contain data
                        string key = row["Name"]?.ToString() ?? "Unnamed";
                        string value = row["Value"]?.ToString() ?? "NoValue";

                        cppFile.WriteLine($"    {{ \"{key}\", \"{value}\" }},");
                    }

                    cppFile.WriteLine("};");
                }

                MessageBox.Show("Data exported to C++ files successfully!");
            }
            else
            {
                MessageBox.Show("DataGrid is not bound to a DataTable.");
            }
        }


        public void EnsureColumnsExist(DataTable dataTable)
        {
            if (!dataTable.Columns.Contains("Name"))
            {
                dataTable.Columns.Add("Name");
            }

            if (!dataTable.Columns.Contains("Value"))
            {
                dataTable.Columns.Add("Value");
            }
        }
    }

}
