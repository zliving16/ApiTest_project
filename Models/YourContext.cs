using Microsoft.EntityFrameworkCore;
 
namespace ApiTest.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Users> users{get;set;}
        public DbSet<Photos> photos{get;set;}
    }
}
