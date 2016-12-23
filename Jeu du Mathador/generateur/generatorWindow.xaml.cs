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
using Microsoft.Win32;
using WinForms = System.Windows.Forms;
using mathador;

namespace generateur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Path = "";

        public MainWindow()
        {
            InitializeComponent();
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath.Text = Path + "\\0_mathador.json";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WinForms.FolderBrowserDialog();
            dialog.ShowDialog();
            filePath.Text = dialog.SelectedPath + "\\" + nbrOfEntry.Text + "_mathador.json";
        }

        private void generateEntries(int nb, string path)
        {
            string text = "{n1:1,n2:2,n3:3,n4:4,n5:5,n:15}";

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
            if (!regex.IsMatch(e.Text))
            {
                int value = int.Parse(((TextBox)e.OriginalSource).Text + e.Text);
                if (value > 1000)
                {
                    ((TextBox)e.OriginalSource).Text = "999";
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(filePath.Text) && filePath.Text[1] == ':')
                if (!string.IsNullOrWhiteSpace(nbrOfEntry.Text))
                {
                    generateEntries(Convert.ToInt32(nbrOfEntry.Text), filePath.Text);
                    error.Text = "";
                    var mathador = new mathador.MainWindow(filePath.Text);
                    this.Close();
                    mathador.ShowDialog();
                }
                else
                    error.Text = "Veuillez saisir un nombre !";
            else
                error.Text = "Ce dossier n'existe pas !";
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NbrOfEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string textChanged = ((TextBox) (e.OriginalSource)).Text;
            long nbrEntry = (string.IsNullOrWhiteSpace(textChanged)) ? 0 : int.Parse(textChanged) ;
            filePath.Text = Path + "\\" + nbrEntry + "_mathador.json";
        }
    }
}
