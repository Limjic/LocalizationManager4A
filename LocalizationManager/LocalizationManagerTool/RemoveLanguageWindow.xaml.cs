using System.Windows;

namespace LocalizationManagerTool
{
    /// <summary>
    /// Interaction logic for RemoveLanguage.xaml
    /// </summary>
    public partial class RemoveLanguageWindow : Window
    {
        public RemoveLanguageWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e) =>
            DialogResult = true;

        private void cancelButton_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;
    }
}
