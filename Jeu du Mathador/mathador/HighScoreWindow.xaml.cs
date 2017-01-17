using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MathadorLibrary;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        private List<DatabaseEntry> list;

        public HighScoreWindow(List<DatabaseEntry> list)
        {
            InitializeComponent();
            this.list = list;
        }
    }
}
