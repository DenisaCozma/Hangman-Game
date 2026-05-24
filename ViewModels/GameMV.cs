using Spanzuratoare.Commands;
using Spanzuratoare.Models;
using Spanzuratoare.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Spanzuratoare.ViewModels
{
    public class GameMV : BaseClass
    {
        public User SelectedUser { get; set; }
        public List<string> HangmanPictures { get; set; }
        public string CurrentHangmanImage => HangmanPictures[Index];
        public ObservableCollection<bool> Hearts { get; set; }

        private MainWindowMV _mainMV;

        private HashSet<string> usedLetters = new HashSet<string>();
        private Dictionary<string, List<string>> WordsCategories;

        private string _selectedCategory;
        private string word;

        private DispatcherTimer timer;
        private bool isGameOver = false;

        private int _level = 1;
        private int _index;
        private int _timeLeft;

        private Random rand = new Random(); 

        public RelayCommand<object> GuessLetterCommand { get; }
        public RelayCommand<object> StatisticsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand HelpCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand CancelCommand { get; }
        public ObservableCollection<string> WordDisplay { get; set; }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                NotifyPropertyChanged(nameof(CurrentHangmanImage));
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged();
                StartGame();
            }
        }

        public int Level
        {
            get => _level;
            set { _level = value; NotifyPropertyChanged(); }
        }

        public int TimeLeft
        {
            get => _timeLeft;
            set { _timeLeft = value; NotifyPropertyChanged(); }
        }

        public GameMV(MainWindowMV mainMV)
        {
            _mainMV = mainMV;
            SelectedUser = mainMV.SelectedUser;

            HangmanPictures = GetHangmanPictures();
            Hearts = new ObservableCollection<bool>(Enumerable.Repeat(true, 6));
            WordDisplay = new ObservableCollection<string>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            GuessLetterCommand = new RelayCommand<object>(GuessLetter, LetterAvailable);
            SelectCategoryCommand = new RelayCommand<string>(SelectCategory);
            StatisticsCommand = new RelayCommand<object>(ShowStatistics);
            HelpCommand = new RelayCommand<object>(Help);

            SaveCommand = new RelayCommand<object>(_ => SaveGame());
            LoadCommand = new RelayCommand<object>(_ => LoadGame());
            NewGameCommand = new RelayCommand<object>(_ => StartGame());
            CancelCommand = new RelayCommand<object>(_ => Cancel());

            SelectedCategory = "All";
        }

        private void StartGame()
        {
            word = ChooseWord();
            isGameOver = false;

            WordDisplay = new ObservableCollection<string>(
                word.Select(c => c == ' ' ? " " : "_")
            );
            NotifyPropertyChanged(nameof(WordDisplay));

            usedLetters.Clear();
            GuessLetterCommand.RaiseCanExecuteChanged();

            Index = 0;
            Hearts = new ObservableCollection<bool>(Enumerable.Repeat(true, 6));
            NotifyPropertyChanged(nameof(Hearts));

            TimeLeft = 30;
            timer.Stop();
            timer.Start();
        }

        private string ChooseWord()
        {
            if (WordsCategories == null)
                LoadWords();

            List<string> words;

            if (SelectedCategory == "All")
            {
                words = WordsCategories.Values.SelectMany(w => w).ToList();
            }
            else
            {
                words = WordsCategories[SelectedCategory];
            }

            return words[rand.Next(words.Count)];
        }

        private void LoadWords()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "words.json");
            WordsCategories = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(File.ReadAllText(path));
        }

        private bool LetterAvailable(object obj)
        {
            if (obj == null) return false;
            return !usedLetters.Contains(obj.ToString().ToUpper());
        }

        private void GuessLetter(object obj)
        {
            string letter = obj.ToString().ToUpper();
            usedLetters.Add(letter);
            GuessLetterCommand.RaiseCanExecuteChanged();

            bool found = false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].ToString() == letter)
                {
                    WordDisplay[i] = letter;
                    found = true;
                }
            }

            if (!found)
            {
                if (Index >= HangmanPictures.Count - 1)
                {
                    isGameOver = true;
                    MessageBox.Show("You lose. The word was " + word);

                    if (!SelectedUser.GamesPerCategory.ContainsKey(SelectedCategory))
                        SelectedUser.GamesPerCategory[SelectedCategory] = 0;

                    SelectedUser.GamesPerCategory[SelectedCategory]++;
                    timer.Stop();
                    Level = 1;
                    _mainMV.SaveUsers();
                    StartGame();
                }
                else
                {
                    Index++;
                    if (Index - 1 < Hearts.Count)
                        Hearts[Index - 1] = false;
                }
            }

            if (!WordDisplay.Contains("_"))
            {
                if (Level == 3)
                {
                    MessageBox.Show("You win!");

                    if (!SelectedUser.WinsPerCategory.ContainsKey(SelectedCategory))
                        SelectedUser.WinsPerCategory[SelectedCategory] = 0;

                    if (!SelectedUser.GamesPerCategory.ContainsKey(SelectedCategory))
                        SelectedUser.GamesPerCategory[SelectedCategory] = 0;

                    SelectedUser.WinsPerCategory[SelectedCategory]++;
                    SelectedUser.GamesPerCategory[SelectedCategory]++;

                    _mainMV.SaveUsers();
                    timer.Stop();
                    Level = 1;
                    StartGame();
                }
                else
                {
                    timer.Stop();
                    Level++;
                    StartGame();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isGameOver) return;

            TimeLeft--;

            if (TimeLeft == 0)
            {
                isGameOver = true;
                timer.Stop();

                if (!SelectedUser.GamesPerCategory.ContainsKey(SelectedCategory))
                    SelectedUser.GamesPerCategory[SelectedCategory] = 0;

                SelectedUser.GamesPerCategory[SelectedCategory]++;
                _mainMV.SaveUsers();

                Level = 1;
                StartGame();
            }
        }

        private void SelectCategory(string category)
        {
            SelectedCategory = category;
        }
        public void SaveGame()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json";

            if (dialog.ShowDialog() == true)
            {
                var state = new GameState
                {
                    Username = SelectedUser.Name,
                    Level = Level,
                    Index = Index,
                    TimeLeft = TimeLeft,
                    SelectedCategory = SelectedCategory,
                    Word = word,
                    WordDisplay = WordDisplay.ToList(),
                    UsedLetters = usedLetters.ToList()
                };

                File.WriteAllText(dialog.FileName, JsonSerializer.Serialize(state));
            }
        }

        public void LoadGame()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json";

            if (dialog.ShowDialog() == true)
            {
                var state = JsonSerializer.Deserialize<GameState>(File.ReadAllText(dialog.FileName));

                if (state.Username != SelectedUser.Name)
                {
                    MessageBox.Show("Nu poți deschide jocul altui utilizator!");
                    return;
                }

                Level = state.Level;
                Index = state.Index;
                TimeLeft = state.TimeLeft;

                _selectedCategory = state.SelectedCategory;
                NotifyPropertyChanged(nameof(SelectedCategory));

                word = state.Word;

                WordDisplay = new ObservableCollection<string>(state.WordDisplay);
                NotifyPropertyChanged(nameof(WordDisplay));

                usedLetters = new HashSet<string>(state.UsedLetters);

                NotifyPropertyChanged(nameof(Index));
                NotifyPropertyChanged(nameof(TimeLeft));
                NotifyPropertyChanged(nameof(CurrentHangmanImage));

                timer.Start();
            }
        }

        private List<string> GetHangmanPictures()
        {
            List<string> pictures = new List<string>(); 
            string exeDir = AppContext.BaseDirectory; 
            string folderPath = Path.Combine(exeDir, "Spanzurat"); 
            if (Directory.Exists(folderPath)) 
            { var files = Directory.GetFiles(folderPath); 
              foreach (string file in files) 
                { 
                    pictures.Add(file); 
                } 
            } 
            return pictures; 
        }

        private void ShowStatistics(object obj)
        {
            var vm = new StatisticsMV(_mainMV);
            var window = new Statistics(_mainMV);
            window.DataContext = vm;
            window.ShowDialog();
        }

        private void Help(object obj)
        {
            new Help().ShowDialog();
        }

        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Game)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}