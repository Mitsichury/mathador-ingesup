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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string filePath;
        private Button firstSelectedButton;
        private Button lastSelectedButton;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string FilePath)
        {
            InitializeComponent();
            filePath = FilePath;
        }

        #region XAMLVariable
        private string _value1;
        public string Value1
        {
            get { return _value1; }
            set
            {
                _value1 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("value1"));
            }
        }
        private string _value2;
        public string Value2
        {
            get { return _value2;  }
            set
            {
                _value2 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("value2"));
            }
        }
        private string _operator;
        public string Operator
        {
            get { return _operator; }
            set
            {
                _operator = value;
                PropertyChanged(this, new PropertyChangedEventArgs("operator"));
            }
        }
        #endregion

        private void ValueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            if (string.IsNullOrWhiteSpace(Value1))
            {
                Value1 = ((TextBlock)button.Content).Text;
                firstSelectedButton = button;
            }
            else if (string.IsNullOrWhiteSpace(Value2))
            {
                Value2 = ((TextBlock)button.Content).Text;
                lastSelectedButton = button;
            }
            Calcul();
        }

        private void OperatorButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            Operator = ((TextBlock)button.Content).Text;
            Calcul();
        }

        private void Calcul()
        {
            if (!string.IsNullOrWhiteSpace(Value1) && !string.IsNullOrWhiteSpace(Value2) && !string.IsNullOrWhiteSpace(Operator))
            {
                int result = 0;
                int value1 = int.Parse(Value1);
                int value2 = int.Parse(Value2);
                switch (Operator)
                {
                    case "+":
                        result = value1 + value2;
                        break;
                    case "-":
                        result = value1 - value2;
                        break;
                    case "/":
                        result = value1 / value2;
                        break;
                    case "X":
                        result = value1 * value2;
                        break;
                }
                ((TextBlock)firstSelectedButton.Content).Text = " ";
                ((TextBlock)lastSelectedButton.Content).Text = result.ToString();
                Value1 = "";
                Value2 = "";
                Operator = "";
            }
        }
    }
}
