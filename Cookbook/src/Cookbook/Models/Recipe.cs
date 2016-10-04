using System.Collections.Generic;

namespace Cookbook.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Serves { get; set; }
        public string UserName { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Instruction> Method { get; set; }
    }
}
