using FileManager.DB;
using FileManager.Models;
using FileManager.ViewModels;
using FileManager.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.Services
{
    public class NavigationService
    {
        private ObservableCollection<FileViewModel> _items;
        private readonly AppDBContext _dbc;
        private readonly SQLiteService _sQLiteService;

        public NavigationService()
        {
            _dbc = new AppDBContext();
            _dbc.Database.EnsureCreated();
            _sQLiteService = new SQLiteService(_dbc);
            _items = new ObservableCollection<FileViewModel>();
        }

        public static NavigationService Default = new();

        public ObservableCollection<FileViewModel> Items => _items;

        public void ChangeFolderPathOrOpenFile(string path)
        {
            // If path is empty, we need to load drives
            if (!string.IsNullOrWhiteSpace(path))
            {
                Navigate(path);
            }
            else
            {
                GetDrives();
            }
        }

        public void Navigate(string path)
        {
            try
            {
                FileAttributes attr = System.IO.File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    OpenFolder(path);
                }
                else
                {
                    OpenFile(path, _sQLiteService);
                }
                ActionService.Default.ChangePath?.Invoke(path);
            }
            catch (Exception)
            {
                MessageBox.Show("This path doesn't exist or folder can't be opened");
            }
        }

        public void OpenFolder(string path)
        {
            var filesAndFolders = Directory.GetFileSystemEntries(path).ToList();

            _items.Clear();
            foreach (var item in filesAndFolders)
            {
                var itemInfo = new FileInfo(item);
                IModel model = null;

                if (itemInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    model = AddItemsHelper.CreateFolder(itemInfo);
                }
                else
                {
                    model = AddItemsHelper.CreateFile(itemInfo);
                }

                _items.Add(new FileViewModel (model));
            }
        }

        public void OpenFile(string path, SQLiteService _sQLiteService)
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

        public void GetDrives()
        {
            var drives = DriveInfo.GetDrives().ToList();

            _items.Clear();

            foreach (var drive in drives)
            {
                Drive model = AddItemsHelper.CreateDrive(drive);
                try
                {
                    int fileCount = Directory.GetFiles(drive.RootDirectory.ToString(), "*", SearchOption.TopDirectoryOnly).Length;
                    model.FileCount = fileCount;
                }
                catch (UnauthorizedAccessException)
                {
                    model.FileCount = 0;
                }
                _items.Add(new FileViewModel(model));
            }
        }
    }
}
