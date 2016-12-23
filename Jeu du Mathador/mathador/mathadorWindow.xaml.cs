using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private Button firstSelectedValue;
        private Button lastSelectedValue;
        private Button SelectedOperator;


        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string FilePath)
        {
            InitializeComponent();
            filePath = FilePath;
            loadMathador();
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
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("errorMessage"));
            }
        }
        private ObservableCollection<string> _historique = new ObservableCollection<string>();
        public ObservableCollection<string> Historique
        {
            get { return _historique; }
            set
            {
                _historique = value;
                PropertyChanged(this, new PropertyChangedEventArgs("historique"));
            }
        }
        #endregion

        private void ValueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            if (button != firstSelectedValue && button != lastSelectedValue)
            {
                if (string.IsNullOrWhiteSpace(Value1))
                {
                    Value1 = ((TextBlock) button.Content).Text;
                    firstSelectedValue = button;
                    firstSelectedValue.Background = Brushes.SaddleBrown;
                }
                else if (string.IsNullOrWhiteSpace(Value2))
                {
                    Value2 = ((TextBlock) button.Content).Text;
                    lastSelectedValue = button;
                    lastSelectedValue.Background = Brushes.SaddleBrown;
                }
                Calcul();
            }
            else
            {
                if (button == firstSelectedValue)
                {
                    Value1 = "";
                    firstSelectedValue.Background = Brushes.Aqua;
                    firstSelectedValue = null;
                }
                else
                {
                    Value2 = "";
                    lastSelectedValue.Background = Brushes.Aqua;
                    lastSelectedValue = null;
                }
            }
        }

        private void OperatorButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            if (button != SelectedOperator)
            {
                if (SelectedOperator != null)
                    SelectedOperator.Background = Brushes.LightGray;
                SelectedOperator = button;
                Operator = ((TextBlock)button.Content).Text;
                SelectedOperator.Background = Brushes.Brown;
            }
            else
            {
                SelectedOperator.Background = Brushes.LightGray;
                SelectedOperator = null;
                Operator = "";
            }
            Calcul();
        }

        private void loadMathador()
        {
            string[] mathadorStrings = File.ReadAllLines(filePath);
            foreach (var mathadorString in mathadorStrings)
            {
                Console.WriteLine(mathadorString);
            }
        }

        private void Calcul()
        {
            if (!string.IsNullOrWhiteSpace(Value1) && !string.IsNullOrWhiteSpace(Value2) && !string.IsNullOrWhiteSpace(Operator))
            {
                ErrorMessage = "";
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
                if (result >= 0)
                {
                    Historique.Add(Value1 + " " + Operator + " " + Value2 + " = " + result);
                    firstSelectedValue.IsEnabled = false;
                    ((TextBlock)firstSelectedValue.Content).Text = " ";
                    ((TextBlock)lastSelectedValue.Content).Text = result.ToString();
                }
                else
                {
                    firstSelectedValue.Background = Brushes.Aqua;
                    ErrorMessage = "Résultat inférieur à 0 impossible !";
                }
                lastSelectedValue.Background = Brushes.Aqua;
                SelectedOperator.Background = Brushes.LightGray;
                firstSelectedValue = null;
                lastSelectedValue = null;
                SelectedOperator = null;
                Value1 = "";
                Value2 = "";
                Operator = "";
            }
        }
    }
}
