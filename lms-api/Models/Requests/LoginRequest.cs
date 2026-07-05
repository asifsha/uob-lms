using System;
namespace lms_api.Models.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";
    }
}

