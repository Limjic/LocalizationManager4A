using System.IO;

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
                    foreach (string value in strArray)
                    {
                        dataTable.Columns.Add(value);
                    }
                }

                strLine = sr.ReadLine();

                while (strLine != null)
                {
                    string[] strArray = strLine.Split(',');
                    dataTable.Rows.Add(strArray);

                    strLine = sr.ReadLine();
                }
            }
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void ExportCSV(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                string strLine;

                strLine = dataTable.Columns[0].ColumnName;

                for (int i = 1; i < dataTable.Columns.Count; ++i)
                {
                    strLine += "," + dataTable.Columns[i].ColumnName;
                }
                sw.WriteLine(strLine);

                for (int i = 0; i < dataTable.Rows.Count; ++i)
                {
                    strLine = "";
                    
                    if (dataTable.Rows[i].ItemArray[0] != null)
                    {
                        strLine = dataTable.Rows[i].ItemArray[0].ToString();
                    }
                    
                    for (int j = 1; j < dataTable.Rows[i].ItemArray.Length; ++j)
                    {
                        strLine += "," + dataTable.Rows[i].ItemArray[j].ToString();
                    }

                    sw.WriteLine(strLine);
                }
            }
        }
    }
}
