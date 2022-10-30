using FileManager.Commands;
using FileManager.Converter;
using FileManager.DB;
using FileManager.Models;
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
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDBContext _dbc;
        private readonly SQLiteService _sQLiteService;
        private readonly InformationHelper _informationHelper = new();
        private string _searchText;
        private string _currentPath;
        private RelayCommand _doubleClickCommand;
        private RelayCommand _backCommand;
        private RelayCommand _searchCommand;
        private RelayCommand _openFileVisitHistoryCommand;
        private string _info;
        private IModel _selectedItem;

        public ObservableCollection<IModel> Items { get; set; }

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
                ChangeFolderPathOrOpenFile((string)value);
            }
        }

        public RelayCommand DoubleClickCommand
        {
            get => _doubleClickCommand ??= new RelayCommand(o => ChangeFolderPathOrOpenFile(SelectedItem.Path));
        }

        public RelayCommand BackCommand
        {
            get => _backCommand ??= new RelayCommand(o => SetPathToParent());
        }

        public RelayCommand SearchCommand
        {
            get => _searchCommand ??= new RelayCommand(o => Search());
        }

        public RelayCommand OpenFileVisitHistoryCommand
        {
            get => _openFileVisitHistoryCommand ??= new RelayCommand(o => OpenFileVisitHistory());
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

        public IModel SelectedItem
        {
            get => _selectedItem; 
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                Info = _informationHelper.GetInfo(value);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            _dbc = new AppDBContext();
            _dbc.Database.EnsureCreated();
            _sQLiteService = new SQLiteService(_dbc);

            Items = new ObservableCollection<IModel>();
            GetDrives();
        }

        private void GetDrives()
        {
            var drives = DriveInfo.GetDrives().ToList();

            Items.Clear();

            foreach (var drive in drives)
            {
                try
                {
                    int fileCount = Directory.GetFiles(drive.RootDirectory.ToString(), "*", SearchOption.TopDirectoryOnly).Length;
                    AddItemsHelper.AddNewDrive(drive, fileCount, Items);
                }
                catch (UnauthorizedAccessException)
                {
                    AddItemsHelper.AddNewDrive(drive, 0, Items);
                }
            }
        }

        private void ChangeFolderPathOrOpenFile(string path)
        {
            // If path is empty, we need to load drives
            if (!string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    FileAttributes attr = System.IO.File.GetAttributes(path);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        OpenItemsHelper.OpenFolder(path, Items);
                    }
                    else
                    {
                        OpenItemsHelper.OpenFile(path, _sQLiteService);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("This path doesn't exist or folder can't be opened");
                }
            }
            else
            {
                GetDrives();
            }

        }                   

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
                    GetDrives();
                    CurrentPath = "";
                }
            }
            else
            {
                MessageBox.Show("Already at top-most directory");
            }
        }

        private void Search()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var items = Items.Where(c => c.Name.ToLower().Contains(SearchText.ToLower())).ToList();

                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(new Folder()
                    {
                        Icon = item.Icon,
                        Name = item.Name,
                        Path = item.Path
                    });
                }
            }
            else
            {
                // If search text is empty: repopulate Items
                ChangeFolderPathOrOpenFile(CurrentPath);
            }
        }

        private void OpenFileVisitHistory()
        {
            FileVisitHistoryWindow fileVisitHistoryWindow = new FileVisitHistoryWindow();
            fileVisitHistoryWindow.ShowDialog();
        }
    }
}
