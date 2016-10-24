using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }

        // Foriegn Key from Recipe
        public int RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
    }
}