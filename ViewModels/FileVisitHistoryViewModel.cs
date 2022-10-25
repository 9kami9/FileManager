using FileManager.DB;
using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.ViewModels
{
    public class FileVisitHistoryViewModel
    {
        private AppDBContext _dbc;
        private SQLiteService _sQLiteService;
        public ObservableCollection<FileDB> Items { get; set; }

        public FileVisitHistoryViewModel()
        {
            _dbc = new AppDBContext();
            _dbc.Database.EnsureCreated();
            _sQLiteService = new SQLiteService(_dbc);

            Items = new ObservableCollection<FileDB>();
            LoadHistory();

        }

        private void LoadHistory()
        {
            Items.Clear();
            foreach (var item in _sQLiteService.ListAll())
            {
                Items.Add(item);
            }
        }
    }
}
