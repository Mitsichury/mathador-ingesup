using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using mathador;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter file = File.CreateText(@"" + path))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.WriteStartArray();
                for (float i = 0; i < nb && !requestStop; i++)
                {
                    mathadorItem item = new mathadorItem((1 + rnd.Next(12)).ToString(), (1 + rnd.Next(12)).ToString(), (1 + rnd.Next(12)).ToString(), (1 + rnd.Next(20)).ToString(), (1 + rnd.Next(20)).ToString(), (1 + rnd.Next(100)).ToString());

                    JObject obj = JObject.FromObject(item, serializer);
                    obj.WriteTo(writer);
                    writer.Flush();
                    if (i % 100 == 0)
                    {
                        Application.Current.Dispatcher.Invoke(
                            new Action(() => { progress.Text = (i / nb * 100) + "%"; }));
                    }
                }

                writer.WriteEndArray();
            }

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