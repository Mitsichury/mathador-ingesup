using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace generateur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            filePath.Text = dialog.SelectedPath + "\\" + nbrOfEntry.Text + "_mathador.json";
        }

        private void generateEntries(int nb, string path)
        {
            string text = "{n:13}";

            List<String> lines = new List<string>();
            for (int i = 0; i < nb; i++)
            {
                lines.Add(text);
            }
            File.AppendAllLines(@"" + path, lines);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            generateEntries(Convert.ToInt32(nbrOfEntry.Text), filePath.Text);
        }
    }
}
}
