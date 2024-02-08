using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication1.Models;

namespace WebApplication1
{

    public class YourDbContext : DbContext
    {
        public YourDbContext() : base("name=YourDbContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }

}