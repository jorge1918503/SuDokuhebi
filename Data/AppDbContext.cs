using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuDokuhebi.Models;

namespace SuDokuhebi.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // PC: DESKTOP-JGJ7PPK
            // Portatil: DESKTOP-MNJ6T8T\SQLEXPRESS
            options.UseSqlServer("Server=DESKTOP-JGJ7PPK;Database=SuDokuhebi;User Id=sa;Password=123456;TrustServerCertificate=True;");
        }
    }
}
