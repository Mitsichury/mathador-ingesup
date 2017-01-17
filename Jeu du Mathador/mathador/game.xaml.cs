using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Button = System.Windows.Controls.Button;
using System.Timers;
using MathadorLibrary;
using Newtonsoft.Json;
using Application = System.Windows.Application;
using Timer = System.Timers.Timer;
using MathadorDatabase;

namespace mathador
{
    /// <summary>
    /// Logique d'interaction pour game.xaml
    /// </summary>
    public partial class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DatabaseHelper db;

        private static readonly Timer TheFinalCountDown = new Timer(1000);
        private readonly string _playerName;
        private readonly Random _rdmIndexMathador = new Random();
        private readonly Dictionary<string, int> _operatorPoints = new Dictionary<string, int>();

        private int _remainingTime;
        private const int PlayingTime = 180;

        private string _filePath;

        private Button _firstSelectedValue;
        private Button _lastSelectedValue;
        private Button _selectedOperator;
        private int _pointsTemp = 0;

        private int _finishedChallengeCount;
        private int _mathadorCount = 0;

        private int _challengeBeginTime;
        private int _totalFinishedChallengeTime;


        public Game(string playerName)
        {
            InitializeComponent();
            db = new DatabaseHelper();

            TheFinalCountDown.Elapsed += OnTimedEvent;
            _playerName = playerName;
            _operatorPoints.Add("+", 1);
            _operatorPoints.Add("-", 2);
            _operatorPoints.Add("/", 3);
            _operatorPoints.Add("X", 1);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _remainingTime--;
            Timer = new TimeSpan(0, _remainingTime / 60, _remainingTime % 60).ToString(@"mm\:ss");
            if (_remainingTime == 0)
            {
                TheFinalCountDown.Stop();
                Application.Current.Dispatcher.Invoke(TimerOut);
            }
        }

        #region XAMLVariable

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

        #region Values

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

        #endregion

        #endregion

        /// <summary>
        /// Fonction recevant l'event click des boutons contenants les valeurs
        /// La fonction effectue les changement de couleurs d'arrière plans des boutons
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
                    ValueShown1 = ((TextBlock) button.Content).Text;
                    _firstSelectedValue = button;
                }
                else if (string.IsNullOrWhiteSpace(ValueShown2))
                {
                    ValueShown2 = ((TextBlock) button.Content).Text;
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
        /// La fonction effectue les changement de couleurs d'arrière plans des boutons
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
                Operator = ((TextBlock) button.Content).Text;
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
        private void LoadMathador()
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

        /// <summary>
        /// Calcul le résultat des deux valeurs séléctionner avec l'opérateur choisi
        /// </summary>
        private void Calcul()
        {
            if (!string.IsNullOrWhiteSpace(ValueShown1) && !string.IsNullOrWhiteSpace(ValueShown2) &&
                !string.IsNullOrWhiteSpace(Operator))
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
                        if (value2 <= 0)
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
                    Historique.Add(new mathadorOper(ValueShown1, ValueShown2, Operator, result.ToString(),
                        _operatorPoints[Operator]));
                    CalcPoint();
                    _firstSelectedValue.IsEnabled = false;
                    ((TextBlock) _firstSelectedValue.Content).Text = " ";
                    ((TextBlock) _lastSelectedValue.Content).Text = result.ToString();
                }
                else
                {
                    ErrorMessage = "Résultat inférieur à 0 impossible !";
                }
                ClearValue();
                CanGoToNext();
            }
        }

        private void ClearValue()
        {
            _firstSelectedValue.Background = Brushes.Aqua;
            _lastSelectedValue.Background = Brushes.Aqua;
            _selectedOperator.Background = Brushes.LightGray;
            _firstSelectedValue = null;
            _lastSelectedValue = null;
            _selectedOperator = null;
            ValueShown1 = "";
            ValueShown2 = "";
            Operator = "";
        }

        private void LaunchGameMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (MathadorCollection.Count == 0)
            {
                ImportFile();
            }
            if (MathadorCollection.Count != 0)
            {
                LoadChallenge();
                _remainingTime = PlayingTime;
                _challengeBeginTime = _remainingTime;
                TheFinalCountDown.Start();
            }
        }

        private void LoadMathadorsValue(mathadorItem item)
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
            ((Window) Parent).Close();
        }

        private void ShowMathadorMenu_OnClick(object sender, RoutedEventArgs e)
        {
            MathadorList mathadorWindow = new MathadorList(MathadorCollection);
            mathadorWindow.Show();
        }

        private void ShowHighScore_OnClick(object sender, RoutedEventArgs e)
        {
            HighScoreWindow highScoreWindowWindow = new HighScoreWindow(db.SelectAll());
            highScoreWindowWindow.Show();
        }

        private void ImportMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ImportFile();
        }

        private void ImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                _filePath = openFileDialog.FileName;
                LoadMathador();
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            _finishedChallengeCount++;
            _totalFinishedChallengeTime += _challengeBeginTime - _remainingTime;
            _challengeBeginTime = _remainingTime;
            Points = IsMathador() ? _pointsTemp + 13 : Points;
            _pointsTemp = Points;
            LoadChallenge();
        }

        private void CanGoToNext()
        {
            if (Value1 == ValueToFind
                || Value2 == ValueToFind
                || Value3 == ValueToFind
                || Value4 == ValueToFind
                || Value5 == ValueToFind)
            {
                NextButton.IsEnabled = true;
            }
            else
                NextButton.IsEnabled = false;
        }

        private void CalcPoint()
        {
            Points += _operatorPoints[Historique.Last().Operator];
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
            DatabaseEntry score = new DatabaseEntry(_playerName, _pointsTemp,
                _totalFinishedChallengeTime / _finishedChallengeCount, _mathadorCount,
                _pointsTemp / _finishedChallengeCount);
            db.Insert(score);
        }

        private void SkipButton_OnClick(object sender, RoutedEventArgs e)
        {
            wizz();
            foreach (var coup in Historique)
            {
                Points -= coup.Points;
            }

            ValueShown1 = " ";
            ValueShown2 = " ";
            Operator = " ";
            LoadChallenge();
        }

        private void LoadChallenge()
        {
            LoadMathadorsValue(MathadorCollection[_rdmIndexMathador.Next(0, MathadorCollection.Count - 1)]);
            ChangeStateValueButton(true);
            Historique.Clear();
            NextButton.IsEnabled = false;
        }


        private void wizz()
        {
        }


        private bool IsMathador()
        {
            if (Historique.Count >= 4)
            {
                for (int i = 0; i < Historique.Count - 1; i++)
                {
                    for (int j = i + 1; j <= Historique.Count - 1; j++)
                    {
                        if (Historique[i].Operator == Historique[j].Operator) return false;
                    }
                }
                _mathadorCount++;
                return true;
            }
            return false;
        }        
    }
}