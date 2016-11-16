using System.ComponentModel.DataAnnotations;

namespace Cookbook.Models.FriendViewModels
{
    public class FindViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
