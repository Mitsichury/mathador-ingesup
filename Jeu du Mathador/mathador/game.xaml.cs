using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;
using UserControl = System.Windows.Controls.UserControl;
using System.Timers;
using System.Windows.Forms.VisualStyles;
using MathadorLibrary;
using Newtonsoft.Json;
using Application = System.Windows.Application;
using Timer = System.Timers.Timer;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour game.xaml
    /// </summary>
    public partial class game : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int remainingTime = 180;
        private string _filePath;
        private static Timer theFinalCountDown = new Timer(1000);
        private Button _firstSelectedValue;
        private Button _lastSelectedValue;
        private Button _selectedOperator;
        private int PLAYING_TIME = 180;

        private Random rdmIndexMathador = new Random();

        private Dictionary<string, int> OperatorPoints = new Dictionary<string, int>();

        public game()
        {
            InitializeComponent();
            theFinalCountDown.Elapsed += OnTimedEvent;
            OperatorPoints.Add("+", 1);
            OperatorPoints.Add("-", 2);
            OperatorPoints.Add("/", 3);
            OperatorPoints.Add("X", 1);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            remainingTime--;
            Timer = new TimeSpan(0, remainingTime/60, remainingTime%60).ToString(@"mm\:ss");
            if (remainingTime == 0)
            {
                theFinalCountDown.Stop();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    TimerOut();
                }));
                
            }
        }

        #region XAMLVariable
        private string _valueShown1;
        public string ValueShown1
        {
            get { return _valueShown1; }
            set
            {
                _valueShown1 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("valueshown1"));
            }
        }
        private string _valueShown2;
        public string ValueShown2
        {
            get { return _valueShown2; }
            set
            {
                _valueShown2 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("valueshown2"));
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
        private ObservableCollection<mathadorOper> _historique = new ObservableCollection<mathadorOper>();
        public ObservableCollection<mathadorOper> Historique
        {
            get { return _historique; }
            set
            {
                _historique = value;
                PropertyChanged(this, new PropertyChangedEventArgs("historique"));
            }
        }
        private ObservableCollection<mathadorItem> _mathadorCollection = new ObservableCollection<mathadorItem>();
        public ObservableCollection<mathadorItem> MathadorCollection
        {
            get { return _mathadorCollection; }
            set
            {
                _mathadorCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MathadorCollection"));
            }
        }
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
            get { return _value2; }
            set
            {
                _value2 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("value2"));
            }
        }
        private string _value3;
        public string Value3
        {
            get { return _value3; }
            set
            {
                _value3 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("value3"));
            }
        }
        private string _value4;
        public string Value4
        {
            get { return _value4; }
            set
            {
                _value4 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("value4"));
            }
        }
        private string _value5;
        public string Value5
        {
            get { return _value5; }
            set
            {
                _value5 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("value5"));
            }
        }
        private string _valueToFind;
        public string ValueToFind
        {
            get { return _valueToFind; }
            set
            {
                _valueToFind = value;
                PropertyChanged(this, new PropertyChangedEventArgs("valueToFind"));
            }
        }

        private string _timer = "00:00";
        public string Timer
        {
            get { return _timer; }
            set
            {
                _timer = value;
                PropertyChanged(this, new PropertyChangedEventArgs("timer"));
            }
        }

        private int _points;        

        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                PropertyChanged(this, new PropertyChangedEventArgs("points"));
            }
        }
        #endregion

        /// <summary>
        /// Fonction recevant l'event click des boutons contenants les valeurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            if (button != _firstSelectedValue && button != _lastSelectedValue)
            {
                if (string.IsNullOrWhiteSpace(ValueShown1))
                {
                    ValueShown1 = ((TextBlock)button.Content).Text;
                    _firstSelectedValue = button;
                }
                else if (string.IsNullOrWhiteSpace(ValueShown2))
                {
                    ValueShown2 = ((TextBlock)button.Content).Text;
                    _lastSelectedValue = button;
                }
                button.Background = Brushes.SaddleBrown;
                Calcul();
            }
            else
            {
                if (button == _firstSelectedValue)
                {
                    ValueShown1 = "";
                    _firstSelectedValue = null;
                }
                else
                {
                    ValueShown2 = "";
                    _lastSelectedValue = null;
                }
                button.Background = Brushes.Aqua;
            }
        }

        /// <summary>
        /// Fonctions recevant l'event click des boutons contenants les opérateurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperatorButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            if (button != _selectedOperator)
            {
                if (_selectedOperator != null)
                    _selectedOperator.Background = Brushes.LightGray;
                _selectedOperator = button;
                Operator = ((TextBlock)button.Content).Text;
                _selectedOperator.Background = Brushes.Brown;
            }
            else
            {
                _selectedOperator.Background = Brushes.LightGray;
                _selectedOperator = null;
                Operator = "";
            }
            Calcul();
        }

        /// <summary>
        /// Fonction au startup
        /// Charge le fichier et stock les mathadors
        /// </summary>
        private void loadMathador()
        {
            List<mathadorItem> items = new List<mathadorItem>();
            if (!string.IsNullOrWhiteSpace(_filePath))
            {               
                using (StreamReader r = new StreamReader(_filePath))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<mathadorItem>>(json);
                }
                foreach (var item in items)
                {
                    MathadorCollection.Add(item);
                }
            }
        }

        private void Calcul()
        {
            if (!string.IsNullOrWhiteSpace(ValueShown1) && !string.IsNullOrWhiteSpace(ValueShown2) && !string.IsNullOrWhiteSpace(Operator))
            {
                bool error = false;
                ErrorMessage = "";
                int result = 0;
                int value1 = int.Parse(ValueShown1);
                int value2 = int.Parse(ValueShown2);
                switch (Operator)
                {
                    case "+":
                        result = value1 + value2;
                        break;
                    case "-":
                        result = value1 - value2;
                        break;
                    case "/":
                        if (value2<=0)
                        {
                            error = true;
                            _firstSelectedValue.Background = Brushes.Aqua;
                            ErrorMessage = "Impossible de diviser par 0 !";
                            break;
                        }
                        result = value1 / value2;
                        break;
                    case "X":
                        result = value1 * value2;
                        break;
                }
                if (result >= 0 && !error)
                {
                    Historique.Add(new mathadorOper(ValueShown1, ValueShown2, Operator, result.ToString()));
                    _firstSelectedValue.Background = Brushes.Aqua;
                    _firstSelectedValue.IsEnabled = false;
                    ((TextBlock)_firstSelectedValue.Content).Text = " ";
                    ((TextBlock)_lastSelectedValue.Content).Text = result.ToString();
                }
                else
                {
                    _firstSelectedValue.Background = Brushes.Aqua;
                    ErrorMessage = "Résultat inférieur à 0 impossible !";
                }
                _lastSelectedValue.Background = Brushes.Aqua;
                _selectedOperator.Background = Brushes.LightGray;
                _firstSelectedValue = null;
                _lastSelectedValue = null;
                _selectedOperator = null;
                ValueShown1 = "";
                ValueShown2 = "";
                Operator = "";
                canGoToNext();
            }
        }

        private void LaunchGameMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (MathadorCollection.Count == 0)
            {
                importFile();
            }
            if (MathadorCollection.Count != 0)
            {
                //loadMathadorsValue(MathadorCollection[rdmIndexMathador.Next(0, MathadorCollection.Count - 1)]);
                loadChallenge();
                remainingTime = PLAYING_TIME;
                theFinalCountDown.Start();
            }
        }

        private void loadMathadorsValue(mathadorItem item)
        {        
            Value1 = item.Value1;
            Value2 = item.Value2;
            Value3 = item.Value3;
            Value4 = item.Value4;
            Value5 = item.Value5;
            ValueToFind = item.Value2;
        }

        private void CloseMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window)Parent).Close();
        }

        private void ShowMathadorMenu_OnClick(object sender, RoutedEventArgs e)
        {
            MathadorList mathadorWindow = new MathadorList(MathadorCollection);
            mathadorWindow.Show();
        }

        private void ImportMenu_OnClick(object sender, RoutedEventArgs e)
        {
            importFile();
        }

        private void importFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                _filePath = openFileDialog.FileName;
                loadMathador();
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            loadChallenge();            
        }

        private void canGoToNext()
        {
            int compteur = 0;
            if(!string.IsNullOrWhiteSpace(Value1)) compteur++;
            if(!string.IsNullOrWhiteSpace(Value2)) compteur++;
            if(!string.IsNullOrWhiteSpace(Value3)) compteur++;
            if(!string.IsNullOrWhiteSpace(Value4)) compteur++;
            if(!string.IsNullOrWhiteSpace(Value5)) compteur++;
            if (compteur == 1)
            {
                CalcPoint();
                NextButton.IsEnabled = true;
            }
        }

        private void CalcPoint()
        {
            List<string> test = new List<string>();
            bool isMathador = true;
            int Pts = 0;
            if (Value1 == ValueToFind || Value2 == ValueToFind || Value3 == ValueToFind || Value4 == ValueToFind || Value5 == ValueToFind)
            {
                foreach (var coup in Historique)
                {
                    if (test.Contains(coup.Operator)) isMathador = false;
                    else
                    {
                        test.Add(coup.Operator);
                    }
                    Pts += OperatorPoints[coup.Operator];
                }
                Points = (isMathador) ? 13 : Pts;
            }
        }

        private void ChangeStateValueButton(bool state)
        {
            ButtonValue1.IsEnabled = state;
            ButtonValue2.IsEnabled = state;
            ButtonValue3.IsEnabled = state;
            ButtonValue4.IsEnabled = state;
            ButtonValue5.IsEnabled = state;
            SkipButton.IsEnabled = state;
        }

        private void TimerOut()
        {
            ChangeStateValueButton(false);
        }

        private void SkipButton_OnClick(object sender, RoutedEventArgs e)
        {
            loadChallenge();
            wizz();
            ValueShown1 = " ";
            ValueShown2 = " ";
            Operator = " ";
        }

        private void loadChallenge()
        {
            loadMathadorsValue(MathadorCollection[rdmIndexMathador.Next(0, MathadorCollection.Count - 1)]);
            ChangeStateValueButton(true);
            Historique.Clear();
            NextButton.IsEnabled = false;
        }

        private void wizz()
        {
            
        }
    }
}

