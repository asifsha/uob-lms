using System;
namespace lms_api.Models.Requests
{
    public class RegisterRequest
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string Role { get; set; } = "Member";
    }
}

