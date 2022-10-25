using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FileManager.Models
{
    [Table("Files")]
    public class FileDB
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("DateVisited")]
        public DateTime DateVisited { get; set; }
    }
}
