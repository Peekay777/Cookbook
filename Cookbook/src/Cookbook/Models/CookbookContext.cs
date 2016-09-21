using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cookbook.Models
{
    public class CookbookContext : DbContext
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
