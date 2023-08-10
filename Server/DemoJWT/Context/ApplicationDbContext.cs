using DemoJWT.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoJWT.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
