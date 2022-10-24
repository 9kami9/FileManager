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
        public static string GetInfo(Drive drive)
        {
            string Info = $"Size: {SizeConverter.Convert(drive.Size)}\n" +
                   $"Type: Drive\n" +
                   $"Available free space: {SizeConverter.Convert(drive.AvailableSpace)}\n" +
                   $"Drive type: {drive.DriveType}\n" +
                   $"Amount of files: {drive.FileCount}";


            return Info;
        }
    }
}
