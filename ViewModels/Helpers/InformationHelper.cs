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
        public static string GetInfo(IModel item)
        {
            var info = new StringBuilder();
            if (item != null)
            {
                info.AppendLine(item.ToString());
                switch (item)
                {
                    case Drive drive:
                        info.AppendLine($"Available free space: {SizeConverter.Convert(drive.AvailableSpace)}");
                        info.AppendLine($"Drive type: {drive.DriveType}");
                        info.AppendLine($"Amount of files: {drive.FileCount}");
                        break;
                    case Folder folder:
                        info.AppendLine($"Amount of files: {folder.AmountOfFiles}");
                        break;
                    case File file:
                        info.AppendLine($"Creation time: {file.DateCreated}");
                        info.AppendLine($"Last access time: {file.LastAccessTime}");
                        info.AppendLine($"Last write time: {file.LastWriteTime}");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                info.AppendLine("No information.");
            }
            return info.ToString();
        }
    }
}
