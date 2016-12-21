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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath;

        public int Value1;
        public int Value2;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string FilePath)
        {
            InitializeComponent();
            filePath = FilePath;
        }

        private void ValueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var textblock = e.OriginalSource as Button;
            if(Value1 == 0)
                Value1 = int.Parse(((TextBlock)textblock.Content).Text);
            else
                Value2 = int.Parse(((TextBlock)textblock.Content).Text);
        }
    }
}
