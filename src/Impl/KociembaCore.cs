using Contract.services;
using Microsoft.Extensions.Configuration;

namespace Impl
{
    /// <summary>
    /// Core implementation of the Kociemba algorithm
    /// </summary>
    public class KociembaCore : IKociembaCore
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public KociembaCore(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["cubeSolveApiRoute"];
        }

        // <inheritdoc />
        public async Task<bool> CheckValidity(string cube)
        {
            var response = await _httpClient.GetAsync($@"{_apiBaseUrl}/solve/{cube}");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to solve the cube.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent == "Invalid cube string";
        }

        // <inheritdoc />
        public async Task<string> Solve(string cube)
        {
            var response = await _httpClient.GetAsync($@"{_apiBaseUrl}/solve/{cube}");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Failed to solve the cube.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
