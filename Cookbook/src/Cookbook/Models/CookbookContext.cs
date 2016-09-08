using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cookbook.Models
{
    public class CookbookContext : DbContext
    {
        private IConfigurationRoot _config;

        public CookbookContext(DbContextOptions options, IConfigurationRoot config)
            :base(options)
        {
            _config = config;
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Method { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:CookbookContextConnection"]);
        }
    }
}
