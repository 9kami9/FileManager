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
        public static Drive CreateDrive(DriveInfo drive)
        {
            return new Drive()
            {
                Name = drive.Name,
                Path = drive.RootDirectory.ToString(),
                Icon = "/Icons/drive.png",
                Size = drive.TotalSize,
                AvailableSpace = drive.AvailableFreeSpace,
                DriveType = drive.DriveType
            };
        }

        public static IModel CreateFile(FileInfo fileInfo)
        {
            return new Models.File()
            {
                Name = fileInfo.Name,
                Path = fileInfo.FullName,
                Icon = "/Icons/file.png",
                Size = fileInfo.Length,
                LastWriteTime = fileInfo.LastWriteTime,
                LastAccessTime = fileInfo.LastAccessTime,
                DateCreated = fileInfo.CreationTime
            };
        }

        public static IModel CreateFolder(FileInfo fileInfo)
        {
            var folder = new Models.Folder()
            { 
                Name = fileInfo.Name,
                Path = fileInfo.FullName,
                Icon = "/Icons/folder.png"
            };

            try
            {
                int amountOfFiles = Directory.GetFiles(fileInfo.FullName.ToString(), "*", SearchOption.TopDirectoryOnly).Length;
                long fileSize = Directory.GetFiles(fileInfo.FullName.ToString(), "*", SearchOption.TopDirectoryOnly).Sum(x => new FileInfo(x).Length);
                folder.Size = fileSize;
                folder.AmountOfFiles = amountOfFiles;
            }
            catch (UnauthorizedAccessException)
            {
                folder.Size = 0;
                folder.AmountOfFiles = 0;
            }

            return folder;
        }


    }
}
