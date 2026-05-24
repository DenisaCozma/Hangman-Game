using Spanzuratoare.Commands;
using Spanzuratoare.Models;
using Spanzuratoare.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace Spanzuratoare.ViewModels
{
    public class MainWindowMV : BaseClass
    {
        public ObservableCollection<User> Users { get; set; }
        public ICommand OpenNewUserCommand { get; }
        public RelayCommand<object> OpenNewGameCommand { get; }

        public RelayCommand<object> DeleteUserCommand { get; }

        private User _selectedUser;
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged();

                OpenNewGameCommand.RaiseCanExecuteChanged();
                DeleteUserCommand.RaiseCanExecuteChanged();
            }
        }
        public MainWindowMV()
        {
            Users = new ObservableCollection<User>();
            LoadUsers();

            OpenNewUserCommand = new RelayCommand<object>(OpenNewUser);
            OpenNewGameCommand = new RelayCommand<object>(OpenNewGame, SelectedUserExists);
            DeleteUserCommand = new RelayCommand<object>(DeleteUser, SelectedUserExists);
        }

        private bool SelectedUserExists(object obj)
        {
            return SelectedUser != null;
        }

        private void OpenNewUser(object obj)
        {
            var vm = new NewUserMV(this);
            var window = new NewUser();
            window.DataContext = vm;
            vm.CloseWindow += () =>
            {
                SaveUsers(); 
                window.Close();
            };
            window.ShowDialog();
        }

        private void OpenNewGame(object obj)
        { 
            var vm = new GameMV(this);
            var window = new Game(this);
            window.DataContext = vm;
            window.ShowDialog();
        }

        public void DeleteUser(object obj)
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
                SaveUsers(); 
            }
        }

        public void SaveUsers()
        {
            var json = JsonSerializer.Serialize(Users);
            File.WriteAllText(filePath, json);
        }

        public void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var users = JsonSerializer.Deserialize<List<User>>(json);

                Users.Clear();

                if (users != null)  
                {
                    foreach (var user in users)
                    {
                        Users.Add(user);
                    }
                }
            }
        }
    }
}
