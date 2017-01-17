using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour launch_menu.xaml
    /// </summary>
    public partial class LaunchMenu : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LaunchMenu()
        {
            InitializeComponent();
        }

        #region XAMLVariable
        private string _playerName;
        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                _playerName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("playerName"));
            }
        }
        #endregion

        private void LaunchMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(PlayerName))
                ((Window) Parent).Content = new Game(PlayerName);
        }

        private void GenerateMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new generateur.GenerateurWindow();
            dialog.ShowDialog();
        }

        private void QuitMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window)Parent).Close();
        }
    }
}
