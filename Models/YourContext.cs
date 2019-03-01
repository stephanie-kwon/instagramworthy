using Microsoft.EntityFrameworkCore;
 
namespace InstagramWorthy.Models
{
    public class YourContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public YourContext(DbContextOptions<YourContext> options) : base(options) { }

        public DbSet<User> users {get;set;}
        public DbSet<Place> places {get;set;}
        public DbSet<Review> reviews {get;set;}
        public DbSet<Like> likes {get;set;}

    }
}


