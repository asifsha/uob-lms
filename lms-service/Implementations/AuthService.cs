using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using lms_data.Entities;
using lms_data.Repositories;
using lms_service.Interfaces;
using lms_service.Helpers;

namespace lms_service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly JwtTokenGenerator _jwt;

        public AuthService(
            IUserRepository repository,
            IPasswordHasher<UserEntity> passwordHasher,
            JwtTokenGenerator jwt)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _jwt = jwt;
        }

        // ---------------- REGISTER ----------------
        public async Task RegisterAsync(string username, string password, string role)
        {
            if (await _repository.ExistsAsync(username))
                throw new InvalidOperationException("Username already exists.");

            var user = new UserEntity
            {
                Username = username,
                Role = role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();
        }

        // ---------------- LOGIN ----------------
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _repository.GetByUsernameAsync(username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password.");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid username or password.");

            return _jwt.Generate(user);
        }
    }
}