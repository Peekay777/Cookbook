using System.Collections.Generic;

namespace Cookbook.Models
{
    public class FriendRequest
    {
        public int RequestId { get; set; }
        public ICollection<Friend> Friends { get; set; }

        public FriendRequest()
        {
            Friends = new List<Friend>();
        }
    }
}
