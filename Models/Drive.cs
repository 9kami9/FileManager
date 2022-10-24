using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class Drive : IModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public long Size { get; set; }
        public long AvailableSpace { get; set; }
        public System.IO.DriveType DriveType { get; set; }
        public int FileCount { get; set; }
    }
}
