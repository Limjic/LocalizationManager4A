using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public DataGrid DataGrid { get => dataGrid; set => dataGrid = value; }
        public void ExportDataGridRoCS(string filePath)
        {
            using (StreamWriter csFile = new StreamWriter(filePath))
            {
                csFile.WriteLine("using System.Collections.Generic;\n");
                csFile.WriteLine("public class Localization\n{\n");
                csFile.WriteLine("\tenum Languages\n\t{");
                csFile.WriteLine("\t\tTRANSLATION");
                for (int i = 1; i < dataGrid.Columns.Count; i++)
                {
                    csFile.WriteLine("\t\t" + dataTable.Columns[i].ColumnName.ToUpper() + ",");
                }
                csFile.WriteLine("\t}\n");
                csFile.WriteLine("\tprivate Dictionary<string, List<string>> locales = new Dictionary<string, List<string>>()");
                csFile.WriteLine("\t{");
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
                    csFile.WriteLine($"\t\t    {{ \"{key}\", new List<string>() {{ {valuesString} }} }},");
                }
                csFile.WriteLine("\t};\n");
                csFile.WriteLine("\tpublic Dictionnary<string, List<string>> Locales { get => locales; set => locales = value; };\n");
                csFile.WriteLine("\tpublic string GetValue(string key, Languages column)\n\t{\n\t\tif (dataMap.TryGetValue(key, out var values)) // Vérifie si la clé existe");
                csFile.WriteLine("\t\t{\n\t\t\tif (column >= 0 && column < values.Count) // Vérifie si l'index est valide\n\t\t\t{\n\t\t\t\treturn values[column];\n\t\t\t}");
                csFile.WriteLine("\t\t\telse\n\t\t\t{\n\t\t\t\tthrow new ArgumentOutOfRangeException(nameof(column), \"Column index is out of range.\");\n\t\t\t}\n\t\t}\n\t\telse\n\t\t{\n\t\t\tthrow new KeyNotFoundException($\"Key not found in the map: {key}\");\n\t\t}\n\t}\n}");

            }
        }
    }
}
