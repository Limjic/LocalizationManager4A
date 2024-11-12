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
        private void OpenCSV(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = ""; // Default file name

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                if (filename.Contains(".odt"))
                {
                    DataGrid dg = new DataGrid();
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string strLine = sr.ReadLine();

                        string[] strArray = strLine.Split(';');

                        foreach (string value in strArray)
                        {
                            dg.Items.Add(value);
                        }

                        while (sr.Peek() >= 0)
                        {
                            strLine = sr.ReadLine();
                            strArray = strLine.Split(';');
                            //dg.Rows.Add(strArray);
                        }
                    }
                }
            }
        }
    }
}
