using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LocalizationManagerTool
{
    public partial class MainWindow
    {
        private void OpenCSV(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string strLine = sr.ReadLine();

                if (strLine != null)
                {
                    string[] strArray = strLine.Split(',');
                    int colCounter = 1;
                    dataTable.Rows.Add();
                    foreach (string value in strArray)
                    {
                        if (colCounter > dataTable.Columns.Count)
                        {
                            dataTable.Columns.Add("language" + colCounter.ToString());
                        }
                    }
                }
            }
        }
    }
}
