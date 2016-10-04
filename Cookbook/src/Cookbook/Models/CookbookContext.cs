using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Models
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
    }
}
