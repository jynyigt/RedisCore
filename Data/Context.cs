using Microsoft.EntityFrameworkCore;
using RedisExampleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExampleCore.Data
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-76J8SD5\\SQLEXPRESS;database=DbRedis;integrated security=true;");

        }
        public DbSet<Person> Persons { get; set; }
    }
}
