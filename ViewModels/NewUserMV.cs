using Spanzuratoare.Commands;
using Spanzuratoare.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Spanzuratoare.ViewModels
{
    public class NewUserMV : BaseClass
    {
        public List<string> Pictures { get; set; }
        public User NewUser { get; set; }
        private MainWindowMV _mainMV;
        private int _index;
        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                NotifyPropertyChanged(nameof(CurrentPicture));
            }
        }

        public string CurrentPicture => Pictures[Index];
        public ICommand NextCommand { get; }
        public ICommand PrevCommand { get; }
        public ICommand SaveCommand { get; }
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyPropertyChanged();
            }
        }

        public NewUserMV(MainWindowMV mainMV)
        {
            _mainMV = mainMV;

            NewUser = new User();
            Pictures = GetPictures();
            Index = 0;
            NextCommand = new RelayCommand<object>(Next);
            PrevCommand = new RelayCommand<object>(Prev);
            SaveCommand = new RelayCommand<object>(Save);
        }

        private List<string> GetPictures()
        {
            List<string> pictures = new List<string>();
            string exeDir = AppContext.BaseDirectory;
            string folderPath = Path.Combine(exeDir, "Pictures");
            if (Directory.Exists(folderPath))
             {
                 var files = Directory.GetFiles(folderPath);

                 foreach (string file in files)
                 {
                     pictures.Add(file); 
                 }
             }

            return pictures;
        }

        private void Next(object obj)
        {
            if (Pictures.Count == 0) return;

            Index = (Index + 1) % Pictures.Count;
        }

        private void Prev(object obj)
        {
            if (Pictures.Count == 0) return;

            Index = (Index - 1 + Pictures.Count) % Pictures.Count;
        }

        public event Action CloseWindow;
        private void Save(object obj)
        {
            if (Pictures.Count == 0) return;

            NewUser.ProfilePicture = CurrentPicture;

            _mainMV.Users.Add(NewUser);

            CloseWindow?.Invoke();
        }
    }

}
