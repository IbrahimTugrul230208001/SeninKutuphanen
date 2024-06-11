using learningASP.NET_CORE.Models;
using System.Net.Http;

namespace learningASP.NET_CORE.Services
{
    public class LibraryService : ILibraryService
    {
            private readonly HttpClient _httpClient;
            private readonly string _apiBaseUrl = "https://localhost:7012/api/EditLibraryAPI";

            public LibraryService(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<Book> AddAsync(Book book)
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Add", book);
                response.EnsureSuccessStatusCode(); // Throw exception if not successful
                return await response.Content.ReadFromJsonAsync<Book>();
            }

            public async Task<Book> UpdateAsync(Book book)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/Update", book);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Book>();
            }

            public async Task DeleteAsync(int bookId)
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Delete/{bookId}");
                response.EnsureSuccessStatusCode(); // Ensure the deletion was successful
            }

            public async Task AddToFavoritesAsync(int bookId, string userName)
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/AddToFavorites", new { BookId = bookId, UserId = userName }); // Assuming you have an AddToFavorites action in your API
                response.EnsureSuccessStatusCode();
            }

            public async Task RemoveBookShowcaseAsync(int bookId, string userName)
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/RemoveBookShowcase", new { BookId = bookId, UserName = userName }); // Assuming you have a RemoveBookShowcase action
                response.EnsureSuccessStatusCode();
            }
        }
    }


