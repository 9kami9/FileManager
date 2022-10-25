using FileManager.Converter;
using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModels.Helpers
{
    public class InformationHelper
    {
        public static string GetInfo(object item)
        {
            string Info;
            if (item is Drive)
            {
                Drive drive = (Drive)item;
                 Info = $"Size: {SizeConverter.Convert(drive.Size)}\n" +
                       $"Type: Drive\n" +
                       $"Available free space: {SizeConverter.Convert(drive.AvailableSpace)}\n" +
                       $"Drive type: {drive.DriveType}\n" +
                       $"Amount of files: {drive.FileCount}";
            }
            else if (item is Folder folder)
            {
                Info = $"Name: {folder.Name}\n" +
                       $"Size: {SizeConverter.Convert((long)folder.Size)}\n" +
                       $"Type: Folder\n" +
                       $"Amount of files: {folder.AmountOfFiles}";
            }
            else if (item is File)
            {
                File file = (File)item;
                Info = $"Name: {file.Name}\n" +
                       $"Size: {SizeConverter.Convert((long)file.Size)}\n" +
                       $"Type: File\n" +
                       $"Creation time: {file.DateCreated}\n" +
                       $"Last access time: {file.LastAccessTime}\n" +
                       $"Last write time: {file.LastWriteTime}";
            }
            else
            {
                Info = "No information";
            }
            return Info;

        }
    }
}
