using System.Collections.Generic;

namespace Cookbook.Models.FriendViewModels
{
    public class RequestViewModel
    {
        public ICollection<Request> SentRequests { get; set; }
        public ICollection<Request> PendingRequests { get; set; }

        public RequestViewModel()
        {
            SentRequests = new List<Request>();
            PendingRequests = new List<Request>();
        }
    }

    public class Request
    {
        public int RequestId { get; set; }
        public string UserName { get; set; }
    }
}
