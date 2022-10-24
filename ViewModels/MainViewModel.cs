using FileManager.Commands;
using FileManager.Converter;
using FileManager.Models;
using FileManager.ViewModels.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace FileManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IModel> Items { get; set; }
        public string CurrentPath { get; set; }
        public long len { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<IModel>();
            GetDrives();
        }

        private string info;
        public string Info 
        { 
            get { return info; } 
            set { info = value; OnPropertyChanged(nameof(Info)); } 
        }
        
        private RelayCommand _singleClickCommand;
        public RelayCommand SingleClickCommand
        {
            get { return _singleClickCommand; }//?? (_singleClickCommand = new RelayCommand(o =>  InformationHelper.GetInfo(selectedItem) )); }
            set { _singleClickCommand = value; }
        }

        private RelayCommand _doubleClickCommand;
        public RelayCommand DoubleClickCommand
        { 
            get { return _doubleClickCommand; } 
            set { _doubleClickCommand = value; } 
        }

        

        private void GetDrives()
        {
            var drives = DriveInfo.GetDrives().ToList();
            len = drives[0].TotalSize;
            len = drives[1].AvailableFreeSpace;
            string type = drives[0].GetType().ToString();

            string convertedSize = SizeConverter.Convert(len);

            CurrentPath = "1";

            Items.Clear();

            foreach (var drive in drives)
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
        }

        private IModel selectedItem;
        public IModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                Info = InformationHelper.GetInfo((Drive)value);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
