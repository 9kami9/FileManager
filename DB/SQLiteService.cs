using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.DB
{
    public class SQLiteService
    {
        private AppDBContext _dbc;

        public SQLiteService(AppDBContext dbc)
        {
            _dbc = dbc;
        }

        public FileDB AddToDb(string fileName, DateTime dateVisited)
        {
            FileDB fileDB = new()
            {
                Name = fileName,
                DateVisited = dateVisited
            };
            _dbc.Files.Add(fileDB);
            _dbc.SaveChanges();
            return fileDB;
        }

        public List<FileDB> ListAll()
        {
            return _dbc.Files.OrderByDescending(p => p.DateVisited).ToList();
        }
    }
}
