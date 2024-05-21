using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Models;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace SwaggerCloneLibrary.Services;

public class ApiAuth(HttpClient httpClient) : IApiAuth
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> Get(string url, string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token is required", nameof(token));
        }

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException($"Error getting data: {response.StatusCode}");
    }
    public async Task<string> CreateRequestTokenAsync(string url, string apiKey)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"{url}{apiKey}");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<RequestTokenResponse>(jsonResponse);
            return tokenResponse.request_token;
        }

        throw new Exception("Failed to create request token.");
    }

    public string ExtractJwtToken(string responseBody)
    {
        string pattern = @"[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+";
        Regex regex = new Regex(pattern);

        Match match = regex.Match(responseBody);

        if (match.Success)
            return match.Value;

        return null;
    }

    public async Task<string> GetSessionIdAsync(string url, string apiKey, string requestToken)
    {
        var client = new HttpClient();
        var content = new StringContent(JsonSerializer.Serialize(new { request_token = requestToken }), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{url}?api_key={apiKey}", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var sessionResponse = JsonSerializer.Deserialize<SessionResponse>(jsonResponse);
            return sessionResponse.session_id;
        }

        throw new HttpRequestException("Failed to create session ID.");
    }
}
