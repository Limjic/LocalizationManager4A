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

                while (strLine != null)
                {
                    string[] strArray = strLine.Split(',');
                    int colCounter = 1;
                    foreach (string value in strArray)
                    {
                        if (colCounter > dataTable.Columns.Count)
                        {
                            dataTable.Columns.Add("language" + colCounter.ToString());
                        }
                        colCounter++;
                    }
                    dataTable.Rows.Add(strArray);

                    strLine = sr.ReadLine();
                }
            }
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
