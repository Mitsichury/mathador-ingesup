﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
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
using System.Windows.Input;
using System.Media;
using System.Windows.Data;
using generateur;
using MahApps.Metro.Controls;
using Solver;
using WMPLib;


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

        string keyboardInput = "";

        static Stream wizzStream = Properties.Resources.wizz;
        static SoundPlayer wizzPlayer = new SoundPlayer(wizzStream);
        static Stream remixStream = Properties.Resources.remix;
        static SoundPlayer remixPlayer = new SoundPlayer(remixStream);


        public Game(string playerName)
        {
            InitializeComponent();
            
            db = new DatabaseHelper();

            TheFinalCountDown.Elapsed += OnTimedEvent;
            _playerName = playerName;
            _operatorPoints.Add("+", 1);
            _operatorPoints.Add("-", 2);
            _operatorPoints.Add("÷", 3);
            _operatorPoints.Add("×", 1);
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
        private mathadorItem _currentMathadorItem;

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
            if (!button.Equals(_firstSelectedValue) && !button.Equals(_lastSelectedValue))
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
                SelectValueButton(button);
                Calcul();
            }
            else
            {
                if (button.Equals(_firstSelectedValue))
                {
                    ValueShown1 = "";
                }
                else
                {
                    ValueShown2 = "";
                }
            }
            CanGoToNext();
        }

        private void SelectValueButton(Button button)
        {
            button.Background = Brushes.Transparent;
            button.BorderBrush = Brushes.Red;
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
                    _selectedOperator.Background = Brushes.Transparent;
                _selectedOperator = button;
                Operator = ((TextBlock) button.Content).Text;
                _selectedOperator.Background = Brushes.Brown;
            }
            else
            {
                _selectedOperator.Background = Brushes.Transparent;
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
                if ((value1 == 0 || value2 == 0) && Operator == "÷")
                {
                    result = -1;
                    ErrorMessage = "Impossible de diviser par 0";
                    error = true;
                }
                else switch (Operator)
                {
                    case "+":
                        result = value1 + value2;
                        break;
                    case "-":
                        result = value1 - value2;
                        break;
                    case "÷":
                        result = value1 / value2;
                        break;
                    case "×":
                        result = value1 * value2;
                        break;
                    default:
                        result = -1;
                        break;
                }
                if (result >= 0 && !error)
                {
                    Historique.Add(new mathadorOper(ValueShown1, ValueShown2, Operator, result.ToString(),
                        _operatorPoints[Operator]));
                    CalcPoint();
                    _firstSelectedValue.IsEnabled = false;
                    SetValue(_firstSelectedValue, " ");
                    SetValue(_lastSelectedValue, result.ToString());
                }
                else
                {
                    if (!error) ErrorMessage = "Resultat inferieur à 0 impossible !";
                }
                ClearValue();
            }
        }

        /// <summary>
        /// Permet de cibler une variable binder sur les ValueButton basé sur leurs noms
        /// </summary>
        /// <param name="button"></param>
        /// <param name="valueToSet"></param>
        private void SetValue(Button button, string valueToSet)
        {
            switch (button.Name)
            {
                case "ButtonValue1":
                    Value1 = valueToSet;
                    break;
                case "ButtonValue2":
                    Value2 = valueToSet;
                    break;
                case "ButtonValue3":
                    Value3 = valueToSet;
                    break;
                case "ButtonValue4":
                    Value4 = valueToSet;
                    break;
                case "ButtonValue5":
                    Value5 = valueToSet;
                    break;
            }
            CanGoToNext();
        }

        private void ClearValue()
        {
            if (_firstSelectedValue != null) _firstSelectedValue.BorderBrush = Brushes.Bisque;
            if (_lastSelectedValue != null) _lastSelectedValue.BorderBrush = Brushes.Bisque;
            if (_selectedOperator != null) _selectedOperator.Background = Brushes.Transparent;
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
                //permet de stopper le timer en cas de fermeture du jeu
                ((Window)Parent).Closing += delegate (object o, CancelEventArgs args) { TheFinalCountDown.Close(); };
            }
        }

        private void LoadMathadorsValue(mathadorItem item)
        {
            _currentMathadorItem = item;
            Value1 = item.Value1;
            Value2 = item.Value2;
            Value3 = item.Value3;
            Value4 = item.Value4;
            Value5 = item.Value5;
            ValueToFind = item.ValueToFind;
        }

        private void CloseMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window) Parent).Close();
        }

        private void ShowMathadorMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (MathadorCollection.Count == 0)
            {
                ImportFile();
            }
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
            DatabaseEntry score = new DatabaseEntry(
                _playerName, 
                _pointsTemp, 
                _finishedChallengeCount>0 ?_totalFinishedChallengeTime/_finishedChallengeCount:0
                , _mathadorCount, _finishedChallengeCount > 0 ? _pointsTemp / _finishedChallengeCount:0
            );
            db.Insert(score);
        }

        private void SkipButton_OnClick(object sender, RoutedEventArgs e)
        {
            Thread wizzSongThread;

            wizzSongThread = new Thread(new ThreadStart(ThreadWizzSong));
            wizzSongThread.Start();

            wizz();
            foreach (var coup in Historique)
            {
                Points -= coup.Points;
            }

            ValueShown1 = " ";
            ValueShown2 = " ";
            Operator = " ";
            ClearValue();
            LoadChallenge();
        }

        private void LoadChallenge()
        {
            ChangeStateValueButton(true);
            LoadMathadorsValue(MathadorCollection[_rdmIndexMathador.Next(0, MathadorCollection.Count - 1)]);
            Historique.Clear();
            NextButton.IsEnabled = false;
        }


        private void wizz()
        {
            Thread wizzy = new Thread(() =>
            {
                int i = 0;
                while (i<5)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ((Window)Parent).Left += 100;
                    }));

                    Thread.Sleep(20);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ((Window)Parent).Left -= 100;
                    }));
                    Thread.Sleep(20);
                    i++;
                }                
            });
            wizzy.Start();
        }

        public static void ThreadWizzSong()
        {
            wizzPlayer.Play();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += ListenCheatCode;
        }

        private void ListenCheatCode(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Console.WriteLine(e.Key);
            keyboardInput = keyboardInput + e.Key.ToString();
            if (keyboardInput.Contains("MUSIC"))
            {
                Thread musicThread;

                musicThread = new Thread(new ThreadStart(ThreadRemixMusic));
                musicThread.Start();

                keyboardInput = "";
            }

            if (keyboardInput.Contains("SOLVE"))
            {
                if (_currentMathadorItem != null)
                {
                    _errorMessage = MathadorSolver.GetMathadors(_currentMathadorItem.valuesToList(), _currentMathadorItem.ValueToFind)[0];
                    PropertyChanged(this, new PropertyChangedEventArgs("errorMessage"));
                }
                keyboardInput = "";
            }
        }

        public static void ThreadRemixMusic()
        {
            remixPlayer.Play();
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

        private void OpenGeneratorMenu_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateurWindow generateurWindow = new GenerateurWindow();
            generateurWindow.Show();
        }
    }
}