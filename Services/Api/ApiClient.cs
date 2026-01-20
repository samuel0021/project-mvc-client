using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.MvcClient.Models.DTO.User;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project.MvcClient.Services.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public Uri BaseAddress => _httpClient.BaseAddress!;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<UserDetailsDto>?> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/user");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserDetailsDto>>(json, _jsonOptions);
        }

        public async Task<UserDetailsDto?> GetUserById(int id)
        {
            var response = await _httpClient.GetAsync($"api/user/{id}");

            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserDetailsDto>();

            if (user is null)
                throw new Exception($"Erro ao buscar usuário: {response.StatusCode}");

            return user ?? new UserDetailsDto();
        }
        
        public async Task<UserDetailsDto?> CreateUserAsync(UserCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/", dto);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDetailsDto>();
        }

        public async Task<UserDetailsDto?> UpdateUserAsync(int id, UserEditDto dto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/user/{id}", dto);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserDetailsDto>();
        }
    }
}
