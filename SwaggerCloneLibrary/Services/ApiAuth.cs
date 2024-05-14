using SwaggerCloneLibrary.Interfaces;

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

}
