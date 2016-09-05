using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.ViewModels
{
    public class RecipeViewModel
    {
        [Required]
        public string Name { get; set; }

        public int Serves { get; set; }

        [MaxLength(30)]
        public ICollection<string> Ingredients { get; set; }

        [MaxLength(20)]
        public ICollection<string> Method { get; set; }
    }
}
