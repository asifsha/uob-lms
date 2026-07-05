using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace lms_api_tests
{

    public class BorrowTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        public class ResonseDto
        {
            public int Id { get; set; }
        }

        private readonly HttpClient _client;

        public BorrowTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Borrow_Book_Flow_Should_Work()
        {

            Console.WriteLine("started..................");
            await TestAuthHelper.AuthenticateAsync(_client, "Librarian");

            // 1. Create book
            var book = new
            {
                title = "Test Book",
                author = "Author",
                isbn = "999",
                publishedYear = 2020,
                isAvailable = true
            };

            var bookResponse = await _client.PostAsJsonAsync("/api/books", book);
            bookResponse.EnsureSuccessStatusCode();

            Console.WriteLine("book created..................");
            var createdBook = await bookResponse.Content.ReadFromJsonAsync<ResonseDto>();

            // 2. Create member
            var member = new
            {
                fullName = "John Doe",
                email = "john@test.com",
                phone = "1234567890"
            };

            var memberResponse=  await _client.PostAsJsonAsync("/api/members", member);
            
            memberResponse.EnsureSuccessStatusCode();
            Console.WriteLine("member created..................");
            var createdMember = await memberResponse.Content.ReadFromJsonAsync<ResonseDto>();

            // 3. Borrow request
            var borrow = new
            {
                bookId = createdBook!.Id,
                memberId = createdMember!.Id
            };

            var response = await _client.PostAsJsonAsync("/api/borrow/borrow", borrow);
            
           
            response.EnsureSuccessStatusCode();
        }
    }
}

