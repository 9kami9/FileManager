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
        private readonly StringBuilder _info = new();

        public string GetInfo(object item)
        {
            _info.Clear();
            switch (item)
            {
                case Drive drive:
                    ItemInfo(drive.Name, drive.Size, "Drive");
                    _info.Append($"Available free space: {SizeConverter.Convert(drive.AvailableSpace)}\n" +
                                 $"Drive type: {drive.DriveType}\n" +
                                $"Amount of files: {drive.FileCount}");
                    break;
                case Folder folder:
                    ItemInfo(folder.Name, (long)folder.Size, "Folder");
                    _info.Append($"Amount of files: {folder.AmountOfFiles}");
                    break;
                case File file:
                    ItemInfo(file.Name, (long)file.Size, "File");
                    _info.Append($"Creation time: {file.DateCreated}\n" +
                                 $"Last access time: {file.LastAccessTime}\n" +
                                 $"Last write time: {file.LastWriteTime}");
                    break;
                default:                    
                    _info.Append("No information.");
                    break;
            }
            return _info.ToString();
        }

        private void ItemInfo(string Name, long Size, string Type)
        {
            _info.Append($"Name: {Name}\n" +
                        $"Size: {Size}\n" +
                        $"Type: {Type}\n");
        }
    }
}
