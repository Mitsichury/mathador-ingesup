﻿using System;
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
using System.Threading;

namespace generateur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Path = "";
        public readonly int MAX_AUTHORIZED = 1000000;

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

        private void generateFileThread(int nb, string path)
        {
            CustomThread customThread = new CustomThread(nb, path);
            Thread t = new Thread(customThread.GenerateEntries);
            t.Start();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (!regex.IsMatch(e.Text))
            {
                int value = formatNbEntry(((TextBox)e.OriginalSource).Text + e.Text);                
                ((TextBox)e.OriginalSource).Text = value.ToString();
                e.Handled = true;
            }
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(filePath.Text) && filePath.Text[1] == ':')
                if (!string.IsNullOrWhiteSpace(nbrOfEntry.Text))
                {
                    generateFileThread(Convert.ToInt32(nbrOfEntry.Text), filePath.Text);
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
            string textChanged = ((TextBox)(e.OriginalSource)).Text;
            int nbrEntry = (string.IsNullOrWhiteSpace(textChanged)) ? 0 : formatNbEntry(textChanged);
            filePath.Text = Path + "\\" + nbrEntry + "_mathador.json";
        }

        private int formatNbEntry(string s)
        {
            int value = 1;
            try
            {
                value = int.Parse(s);
                if(value > MAX_AUTHORIZED)
                {
                    value = MAX_AUTHORIZED;
                }
            }
            catch (OverflowException)
            {
                value = MAX_AUTHORIZED;
            }

            return value;
        }
    }


    public class CustomThread
    {
        private int nb;
        private string path;

        public CustomThread(int nb, string path)
        {
            this.nb = nb;
            this.path = path;
        }

        public void GenerateEntries()
        {
            /*
            - 3 nombres entre 1 et 12
            - 2 nombres entre 1 et 20
            - le nombre cible entre 1 et 100
            */
            Random rnd = new Random();


            List<int> numbers = new List<int>();
            List<string> lines = new List<string>();
            for (int i = 0; i < nb; i++)
            {
                numbers.Add(1 + rnd.Next(12));
                numbers.Add(1 + rnd.Next(12));
                numbers.Add(1 + rnd.Next(12));
                numbers.Add(1 + rnd.Next(20));
                numbers.Add(1 + rnd.Next(20));
                numbers.Add(1 + rnd.Next(100));
                lines.Add(String.Join(", ", numbers.ToArray()));
                numbers.Clear();
                if (lines.Count > 5000)
                {
                    File.AppendAllLines(@"" + path, lines);
                    lines.Clear();
                }
            }

            File.AppendAllLines(@"" + path, lines);
        }
    }

}