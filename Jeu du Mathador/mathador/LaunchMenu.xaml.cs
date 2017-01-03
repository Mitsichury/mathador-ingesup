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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour launch_menu.xaml
    /// </summary>
    public partial class LaunchMenu : UserControl
    {
        public LaunchMenu()
        {
            InitializeComponent();
        }

        private void LaunchMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window) Parent).Content = new game();
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
