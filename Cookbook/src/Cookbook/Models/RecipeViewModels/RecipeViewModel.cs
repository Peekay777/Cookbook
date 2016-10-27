using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cookbook.Models.RecipeViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1.0,30.0)]
        public int Serves { get; set; }

        public string UserName { get; set; }

        [Required]
        public ICollection<IngredientViewModel> Ingredients { get; set; }
        
        [Required]
        public ICollection<InstructionViewModel> Method { get; set; }
    }
}
