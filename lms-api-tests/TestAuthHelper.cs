using System;
using lms_api.Models.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace lms_api_tests
{
	public class TestAuthHelper
	{
        public static async Task AuthenticateAsync(HttpClient client, string role)
        {
            var username = $"user-{Guid.NewGuid():N}";
            // ---------------- REGISTER ----------------
            var register = new
            {
                username = username,
                password = "123456",
                role = role
            };

            var regResponse = await client.PostAsJsonAsync("/api/auth/register", register);
            regResponse.EnsureSuccessStatusCode();

            var login = new
            {
                username = username,
                password = "123456"
            };

            var response = await client.PostAsJsonAsync("/api/auth/login", login);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result!.Token);
        }
    }
}

