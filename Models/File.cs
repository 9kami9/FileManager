using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class File : BaseModel
    {
        public File() : base(nameof(File))
        {

        }

        public DateTime DateCreated { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
    }
}