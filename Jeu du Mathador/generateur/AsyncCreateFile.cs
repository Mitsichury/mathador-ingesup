using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace generateur
{
    class AsyncCreateFile
    {
        private float nb;
        private string path;
        private Button button;
        private TextBlock progress;

        private bool requestStop;

        public AsyncCreateFile(float nb, string path, Button button, TextBlock progress)
        {
            this.nb = nb;
            this.path = path;
            this.button = button;
            this.progress = progress;
            requestStop = false;
        }

        public void stop()
        {
            requestStop = true;
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
            for (float i = 0; i < nb && !requestStop; i++)
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
                if(lines.Count % 100 == 0)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => { progress.Text = (i / nb * 100) + "%"; }));
                }
            }

            File.AppendAllLines(@"" + path, lines);
            if (!requestStop)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    button.IsEnabled = true;
                    progress.Text = "100%";
                    button.Content = "Générer";
                }));
            }
        }
    }
}
