namespace Cookbook.Models
{
    public enum FriendStatus
    {
        Pending = 1,    // Waiting for sent confirmation
        Requested = 2,  // Received request waiting to be accepted
        Confirmed = 3,
        Cancelled = 4
    };

    public class Friend
    {
        public string UserId { get; set; }
        public CookbookUser User { get; set; }

        public int RequestId { get; set; }
        public FriendRequest Request { get; set; }

        public FriendStatus Status { get; set; }
    }
}
    