using CarWash.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Areas.Api.Models
{
    public class MyProjectContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Server=srisuk.database.windows.net; Database=CarWash; User ID=srisuk013; Password=Srisuk1234");
        }
        public DbSet<JobHistory> histories { get; set; }
 /*       public DbSet<Package> package { get; set; }
        public DbSet<Job> jobs { get; set; }
        public DbSet<Car>car { get; set; }*/
    
    }
}
