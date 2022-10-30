using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModels.Helpers
{
    public class AddItemsHelper
    {
        public static void AddNewDrive(DriveInfo drive, int fileCount, ObservableCollection<IModel> Items)
        {
            Items.Add(new Drive()
            {
                Name = drive.Name,
                Path = drive.RootDirectory.ToString(),
                Icon = "/Icons/drive.png",
                Size = drive.TotalSize,
                AvailableSpace = drive.AvailableFreeSpace,
                DriveType = drive.DriveType,
                FileCount = fileCount
            });
        }

        public static void AddFileToListItem(FileInfo fileInfo, ObservableCollection<IModel> Items)
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

        public static void AddFolderToListItem(FileInfo fileInfo, ObservableCollection<IModel> Items)
        {
            try
            {
                int amountOfFiles = Directory.GetFiles(fileInfo.FullName.ToString(), "*", SearchOption.TopDirectoryOnly).Length;
                long fileSize = Directory.GetFiles(fileInfo.FullName.ToString(), "*", SearchOption.TopDirectoryOnly).Sum(x => new FileInfo(x).Length);
                AddNewFolder(fileInfo, amountOfFiles, fileSize, Items);
            }
            catch (UnauthorizedAccessException)
            {
                AddNewFolder(fileInfo, 0, 0, Items);
            }
        }

        private static void AddNewFolder(FileInfo fileInfo, int amountOfFiles, long size, ObservableCollection<IModel> Items)
        {
            Items.Add(new Folder()
            {
                Name = fileInfo.Name,
                Path = fileInfo.FullName,
                Icon = "/Icons/folder.png",
                AmountOfFiles = amountOfFiles,
                Size = size
            });
        }
    }
}
