using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using FluentAssertions;
using lms_api.Models.Responses;

namespace lms_api_tests
{
    public class AuthTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_Then_Login_Should_Return_Token()
        {
            // ---------------- REGISTER ----------------
            var register = new
            {
                username = "testuser",
                password = "123456",
                role = "Member"
            };

            var regResponse = await _client.PostAsJsonAsync("/api/auth/register", register);
            regResponse.EnsureSuccessStatusCode();

            // ---------------- LOGIN ----------------
            var login = new
            {
                username = "testuser",
                password = "123456"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", login);
            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            loginResult.Should().NotBeNull();
            loginResult!.Token.Should().NotBeNullOrWhiteSpace();
        }
    }
}

