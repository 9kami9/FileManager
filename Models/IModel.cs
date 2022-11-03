using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public interface IModel
    {
        string Name { get; set; }
        string Path { get; set; }
        string Icon { get; set; }
        long Size { get; set; }
        string Type { get; }
    }
}
