using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        public void ExportDataGridRoCS(string filePath)
        {
            using (StreamWriter csFile = new StreamWriter(filePath))
            {
                csFile.WriteLine("using System.Collections.Generic;\nusing UnityEngine;\n");
                csFile.WriteLine("public Dictionary<string, List<string>> locales = new Dictionary<string, List<string>>()");
                csFile.WriteLine("{");
            }
        }
    }
}
