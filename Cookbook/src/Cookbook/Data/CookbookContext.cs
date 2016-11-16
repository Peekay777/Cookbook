using Cookbook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cookbook.Data
{
    public class CookbookContext : IdentityDbContext<CookbookUser>
    {
        public CookbookContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Method { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FriendRequest>()
                .HasKey(fr => fr.RequestId);

            builder.Entity<Friend>()
                .HasKey(f => new { f.UserId, f.RequestId });

            builder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.UserId);

            builder.Entity<Friend>()
                .HasOne(f => f.Request)
                .WithMany(r => r.Friends)
                .HasForeignKey(f => f.RequestId);
        }
    }
}
