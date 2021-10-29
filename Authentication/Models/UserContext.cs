using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
        {

        }
        public UserContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

    }
}
