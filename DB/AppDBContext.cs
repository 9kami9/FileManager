using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManager.Models;
using Microsoft.EntityFrameworkCore;


namespace FileManager.DB
{
    public class AppDBContext : DbContext
    {
        public DbSet<FileDB> Files { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Database.db");
        }
    }
}
