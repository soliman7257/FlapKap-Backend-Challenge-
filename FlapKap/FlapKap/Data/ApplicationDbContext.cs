
using FlapKap.Data;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        //////////////////////////////////////



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

    
    }
}





