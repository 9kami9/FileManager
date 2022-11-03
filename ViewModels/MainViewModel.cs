using FileManager.Commands;
using FileManager.Converter;
using FileManager.DB;
using FileManager.Models;
using FileManager.Services;
using FileManager.View;
using FileManager.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _searchText;
        private string _currentPath;
        private string _info;
        private RelayCommand _backCommand;
        private RelayCommand _searchCommand;
        private RelayCommand _openFileVisitHistoryCommand;

        public MainViewModel()
        {
            ActionService.Default.ChangePath += c => 
            {
                _currentPath = c;
                OnPropertyChanged(nameof(CurrentPath));
            };
            ActionService.Default.ChangeInfo += c => Info = c;

            NavigationService.Default.GetDrives();
        }

        public ObservableCollection<FileViewModel> Items => NavigationService.Default.Items;

        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string CurrentPath
        {
            get => _currentPath; 
            set
            {
                _currentPath = value;
                OnPropertyChanged(nameof(CurrentPath));
                NavigationService.Default.ChangeFolderPathOrOpenFile((string)value);
            }
        }

        public string Info
        {
            get => _info; 
            set 
            {
                _info = value;
                OnPropertyChanged(nameof(Info)); 
            }
        }

        public RelayCommand BackCommand 
            => _backCommand ??= new RelayCommand(o => SetPathToParent());
        
        public RelayCommand SearchCommand
            => _searchCommand ??= new RelayCommand(o => Search());

        public RelayCommand OpenFileVisitHistoryCommand
            => _openFileVisitHistoryCommand ??= new RelayCommand(o => OpenFileVisitHistory());                 

        private void SetPathToParent()
        {
            if (!string.IsNullOrWhiteSpace(CurrentPath))
            {
                if (Directory.GetParent(CurrentPath) != null)
                {
                    CurrentPath = Directory.GetParent(CurrentPath).ToString();
                }
                else
                {
                    NavigationService.Default.GetDrives();
                    CurrentPath = "";
                }
            }
            else
            {
                MessageBox.Show("Already at top-most directory");
            }
            Info = InformationHelper.GetInfo(null);
        }

        private void Search()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var items = Items.Where(c => c.Name.ToLower().Contains(SearchText.ToLower())).ToList();
                Items.Clear();

                foreach (var item in items)
                {
                    IModel model = new Folder()
                    {
                        Icon = item.Icon,
                        Name = item.Name,
                        Path = item.Path
                    };
                    Items.Add(new FileViewModel(model));
                }
            }
            else
            {
                // If search text is empty: repopulate Items
                NavigationService.Default.ChangeFolderPathOrOpenFile(CurrentPath);
            }
        }

        private void OpenFileVisitHistory()
        {
            FileVisitHistoryWindow fileVisitHistoryWindow = new();
            fileVisitHistoryWindow.ShowDialog();
        }
    }
}
