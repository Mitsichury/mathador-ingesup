using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using mathador.Annotations;
using MathadorLibrary;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public HighScoreWindow(List<DatabaseEntry> list)
        {
            InitializeComponent();
            list.ForEach(x => HighScoreList.Add(x));
        }

        private ObservableCollection<DatabaseEntry> _HighScoreList = new ObservableCollection<DatabaseEntry>();
        public ObservableCollection<DatabaseEntry> HighScoreList
        {
            get { return _HighScoreList; }
            set
            {
                _HighScoreList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("highScoreList"));
            }
        }
    }
}
