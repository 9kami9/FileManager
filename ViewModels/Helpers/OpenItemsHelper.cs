using FileManager.DB;
using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModels.Helpers
{
    public class OpenItemsHelper
    {
        public static void OpenFile(string path, SQLiteService _sQLiteService)
        {
            var fileName = Path.GetFileName(path);
            DateTime dateVisited = DateTime.Now;
            var p = new Process();

            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            // Opens file
            p.Start();

            // Add record to Db
            _sQLiteService.AddToDb(fileName, dateVisited);
        }

        public static void OpenFolder(string path, ObservableCollection<IModel> Items)
        {
            var filesAndFolders = Directory.GetFileSystemEntries(path).ToList();

            Items.Clear();
            foreach (var item in filesAndFolders)
            {
                var itemInfo = new FileInfo(item);

                if (itemInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    AddItemsHelper.AddFolderToListItem(itemInfo, Items);
                }
                else
                {
                    AddItemsHelper.AddFileToListItem(itemInfo, Items);
                }
            }
        }
    }
}
