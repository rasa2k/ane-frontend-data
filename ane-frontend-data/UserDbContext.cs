using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace aneFrontendData
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        protected UserDbContext()
        {
        }
        public DbSet<User> Users { get; set; }
    }
}