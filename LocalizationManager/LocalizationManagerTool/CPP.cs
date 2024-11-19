using System.Collections.Generic;
using System.Data;
using System.IO;
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
                // Ensure the DataTable has at least one column
                EnsureColumnsExist(dataTable);

                // Open the .h file for writing
                using (StreamWriter hFile = new StreamWriter(hFilePath))
                {
                    hFile.WriteLine("#include <map>");
                    hFile.WriteLine("#include <string>");
                    hFile.WriteLine("#include <vector>");
                    hFile.WriteLine("using namespace std;");
                    hFile.WriteLine(); // Ceci est un retour a la ligne
                    hFile.WriteLine("class LanguageDataMap");
                    hFile.WriteLine("{");
                    hFile.WriteLine("public:");
                    hFile.WriteLine("\tLanguageDataMap();");
                    hFile.WriteLine();
                    hFile.WriteLine("\tmap<string, vector<string>> GetDataMap();");
                    hFile.WriteLine("\tvoid SetDataMap(map<string, vector<string>> _dataMap);");
                    hFile.WriteLine("private:");
                    hFile.WriteLine("\tmap<string, vector<string>> dataMap;");
                    hFile.WriteLine("};");
                }

                // Open the .cpp file for writing
                using (StreamWriter cppFile = new StreamWriter(cppFilePath))
                {
                    cppFile.WriteLine("#include \"file.h\"");
                    cppFile.WriteLine();
                    cppFile.WriteLine("LanguageDataMap::LanguageDataMap()");
                    cppFile.WriteLine("{");

                    // Loop through the DataTable and write data to the map initialization in .cpp
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Use the first column as the key, and the rest as the values
                        string key = row[0]?.ToString() ?? "Unnamed";
                        var values = new List<string>();

                        // Gather all values except the key
                        for (int col = 1; col < dataTable.Columns.Count; col++)
                        {
                            string value = row[col]?.ToString() ?? "NoValue";
                            values.Add(value);
                        }

                        // Convert the values into a C++ vector representation
                        string valuesString = string.Join(", ", values.ConvertAll(v => $"\"{v}\""));
                        cppFile.WriteLine($"    {{ \"{key}\", {{ {valuesString} }} }},");
                    }

                    cppFile.WriteLine("};");
                    cppFile.WriteLine("}");
                    cppFile.WriteLine();
                    cppFile.WriteLine("map<string, vector<string>> LanguageDataMap::GetDataMap()");
                    cppFile.WriteLine("{");
                    cppFile.WriteLine("\treturn dataMap;");
                    cppFile.WriteLine("}");
                    cppFile.WriteLine();
                    cppFile.WriteLine("void LanguageDataMap::SetDataMap(map<string, vector<string>> _dataMap)");
                    cppFile.WriteLine("{");
                    cppFile.WriteLine("\tdataMap = _dataMap;\r\n");
                    cppFile.WriteLine("}");
                    cppFile.WriteLine();
                    cppFile.WriteLine("std::string LanguageDataMap::GetValue(const std::string& key, LanguageColumns column)");
                    cppFile.WriteLine("{");
                    cppFile.WriteLine("\tauto it = dataMap.find(key);");
                    cppFile.WriteLine("\tif (it != dataMap.end())");
                    cppFile.WriteLine("\t{");
                    cppFile.WriteLine("\t\tconst auto& values = it->second;");
                    cppFile.WriteLine();
                    cppFile.WriteLine("\t\tif (static_cast<size_t>(column) < values.size())");
                    cppFile.WriteLine("\t\t{");
                    cppFile.WriteLine("\t\t\treturn values[column];");
                    cppFile.WriteLine("\t\t}");
                    cppFile.WriteLine("\t\telse");
                    cppFile.WriteLine("\t\t{");
                    cppFile.WriteLine("\t\t\tthrow std::out_of_range(\"Column index is out of range.\");");
                    cppFile.WriteLine("\t\t}");
                    cppFile.WriteLine("\t}");
                    cppFile.WriteLine("\telse");
                    cppFile.WriteLine("\t{");
                    cppFile.WriteLine("\t\tthrow std::invalid_argument(\"Key not found in the map.\");");
                    cppFile.WriteLine("\t}");
                    cppFile.WriteLine("}");
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
            // Ensure at least one column exists
            if (dataTable.Columns.Count == 0)
            {
                dataTable.Columns.Add("Column1");
            }
        }
    }
}
