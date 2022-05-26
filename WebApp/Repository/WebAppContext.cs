using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Repository
{
    public class WebAppContext : DbContext
    {
        //public WebAppContext (DbContextOptions<WebAppContext> options)
        //    : base(options)
        //{
        //}

        private const string connectionString = "server=localhost;port="

        public WebAppContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User>? User { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Message> Message { get; set; }
    }
}
