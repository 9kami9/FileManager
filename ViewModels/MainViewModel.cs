using FileManager.Commands;
using FileManager.Converter;
using FileManager.DB;
using FileManager.Models;
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
        private AppDBContext _dbc;

        private SQLiteService _sQLiteService;

        public ObservableCollection<IModel> Items { get; set; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        private string _currentPath;
        public string CurrentPath
        {
            get { return _currentPath; }
            set
            {
                _currentPath = value;
                OnPropertyChanged("CurrentPath");
                ChangeFolderPathOrOpenFile((string)value);
            }
        }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand ?? (_backCommand = new RelayCommand(o => SetPathToParent())) ; }
        }

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand(o => Search()));
            }
        }

        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; OnPropertyChanged(nameof(Info)); }
        }

        private IModel _selectedItem;
        public IModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                Info = InformationHelper.GetInfo(value);
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
                    Items.Add(new Drive()
                    {
                        Name = drive.Name,
                        Path = drive.RootDirectory.ToString(),
                        Icon = "/Icons/drive.png",
                        Size = drive.TotalSize,
                        AvailableSpace = drive.AvailableFreeSpace,
                        DriveType = drive.DriveType,
                        FileCount = Directory.GetFiles(drive.RootDirectory.ToString(), "*", SearchOption.TopDirectoryOnly).Count()
                    });
                }
                catch (UnauthorizedAccessException)
                {
                    Items.Add(new Drive()
                    {
                        Name = drive.Name,
                        Path = drive.RootDirectory.ToString(),
                        Icon = "/Icons/drive.png",
                        Size = drive.TotalSize,
                        AvailableSpace = drive.AvailableFreeSpace,
                        DriveType = drive.DriveType,
                        FileCount = 0
                    });
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
                        OpenFolder(path);

                    else
                        OpenFile(path);
                }
                catch
                {
                    MessageBox.Show("This path doesn't exist or folder can't be opened");
                }
            }
            else
                GetDrives();

        }

        private void OpenFile(string path)
        {
            var fileName = Path.GetFileName(path);
            DateTime dateVisited = DateTime.Now;
            var p = new Process();

            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            // Opens file
            p.Start();
            
            // Add record to Db
            _sQLiteService.AddToDb(fileName, dateVisited);
        }

        private void OpenFolder(string path)
        {
            var filesAndFolders = Directory.GetFileSystemEntries(path).ToList();

            Items.Clear();
            foreach (var item in filesAndFolders)
            {
                var itemInfo = new FileInfo(item);

                if (itemInfo.Attributes.HasFlag(FileAttributes.Directory))
                    AddFolderToListItem(itemInfo);  
                else
                    AddFileToListItem(itemInfo); 
            }
        }

        private void AddFileToListItem(FileInfo fileInfo)
        {
            Items.Add(new Models.File()
            {
                Name = fileInfo.Name,
                Path = fileInfo.FullName,
                Icon = "/Icons/file.png",
                Size = fileInfo.Length,
                LastWriteTime = fileInfo.LastWriteTime,
                LastAccessTime = fileInfo.LastAccessTime,
                DateCreated = fileInfo.CreationTime
            });
        }

        private void AddFolderToListItem(FileInfo fileInfo)
        {
            try
            {
                Items.Add(new Folder()
                {
                    Name = fileInfo.Name,
                    Path = fileInfo.FullName,
                    Icon = "/Icons/folder.png",
                    AmountOfFiles = Directory.GetFiles(fileInfo.FullName.ToString(), "*", SearchOption.TopDirectoryOnly).Count(),
                    Size = Directory.GetFiles(fileInfo.FullName.ToString(), "*", SearchOption.TopDirectoryOnly).Sum(x => new FileInfo(x).Length)
                });
            }
            catch (UnauthorizedAccessException)
            {
                Items.Add(new Folder()
                {
                    Name = fileInfo.Name,
                    Path = fileInfo.FullName,
                    Icon = "/Icons/folder.png",
                    AmountOfFiles = 0,
                    Size = 0
                });
            }
        }

        private void SetPathToParent()
        {
            if (!string.IsNullOrWhiteSpace(CurrentPath))
            {
                if (Directory.GetParent(CurrentPath) != null)
                    CurrentPath = Directory.GetParent(CurrentPath).ToString();
                else
                {
                    GetDrives();
                    CurrentPath = "";
                }
            }
            else
                MessageBox.Show("Already at top-most directory");
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
                // If search text is empty: repopulate Items
                ChangeFolderPathOrOpenFile(CurrentPath);
        }

    }
}
