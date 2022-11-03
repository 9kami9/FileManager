using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Services
{
    public class ActionService
    {
        public Action<string> ChangePath { get; set; }
        public Action<string> ChangeInfo { get; set; }

        public static ActionService Default = new();
    }
}
