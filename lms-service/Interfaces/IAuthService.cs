using System;
namespace lms_service.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(string username, string password, string role);

        Task<string> LoginAsync(string username, string password);
    }
}

