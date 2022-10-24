using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileManager.Converter
{
     public class SizeConverter
    {
        public static string Convert(long value)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = value;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:.00} {sizes[order]}";
        }

    }
}
