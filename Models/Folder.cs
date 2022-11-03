using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class Folder : BaseModel
    {
        public Folder() : base(nameof(Folder))
        {

        }

        public int AmountOfFiles { get; set; }
    }
}
