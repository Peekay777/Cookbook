using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public class Recipes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Serves { get; set; }
        public ICollection<string> Ingredients { get; set; }
        public ICollection<string> Method { get; set; }
    }
}
