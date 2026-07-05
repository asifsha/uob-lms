using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace lms_api_tests
{
    public class BookTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BookTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_Book_Should_Return_Success()
        {
            await TestAuthHelper.AuthenticateAsync(_client, "Librarian");

            var book = new
            {
                title = "Clean Code",
                author = "Robert Martin",
                isbn = "123456",
                publishedYear = 2008,
                isAvailable = true
            };

            var response = await _client.PostAsJsonAsync("/api/books", book);

            response.EnsureSuccessStatusCode();
        }
    }
}

