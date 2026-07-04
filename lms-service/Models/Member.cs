using System;
namespace lms_service.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime MembershipDate { get; set; }
    }
}

