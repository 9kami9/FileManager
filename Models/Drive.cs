using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class Drive : BaseModel
    {
        public Drive() : base(nameof(Drive))
        {

        }

        public long AvailableSpace { get; set; }
        public System.IO.DriveType DriveType { get; set; }
        public int FileCount { get; set; }
    }
}
