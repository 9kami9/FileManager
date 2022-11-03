using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public abstract class BaseModel : IModel
    {
        protected BaseModel(string type)
        {
            Type = type;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public long Size { get; set ; }
        public string Type { get; }

        public override string ToString()
        {
            return $"Name: {Name}\nSize: {Size}\nType: {Type}";
        }
    }
}
