using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cookbook.Models
{
    public class CookbookUser : IdentityUser
    {
        public ICollection<Friend> Friends { get; set; }

        public CookbookUser()
        {
            Friends = new List<Friend>();
        }
    }
}
