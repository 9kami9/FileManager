using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class File : IModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public double Size { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
