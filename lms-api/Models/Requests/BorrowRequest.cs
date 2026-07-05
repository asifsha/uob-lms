using System;
namespace lms_api.Models.Requests
{
    public class BorrowRequest
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
    }
}

