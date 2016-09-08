using Microsoft.EntityFrameworkCore;

namespace Cookbook.Models
{
    public class CookbookContext : DbContext
    {
        public CookbookContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Method { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDb;Database=Cookbook;Trusted_Connection=true;MultipleActiveResultSets=true;");
        }
    }
}
