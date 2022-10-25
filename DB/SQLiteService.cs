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
            FileDB fileDB = new FileDB();
            fileDB.Name = fileName;
            fileDB.DateVisited = dateVisited;
            return AddToDb(fileDB);
        }

        private FileDB AddToDb(FileDB fileVisit)
        {
            _dbc.Files.Add(fileVisit);
            _dbc.SaveChanges();
            return fileVisit;
        }

        public List<FileDB> ListAll()
        {
            return _dbc.Files.OrderByDescending(p => p.DateVisited).ToList();
        }
    }
}
