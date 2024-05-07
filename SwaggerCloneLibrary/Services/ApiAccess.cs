using System.Net;
using System.Text.Encodings.Web;

namespace SwaggerCloneLibrary.Services;

public class ApiAccess
{
    private readonly HttpClient _httpClient = new();
    public async Task<string> CallApi(string url)
    {

        if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return "Error: Invalid or missing URL.";
        }

        var response = await _httpClient.GetAsync(url);

        try
        {
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            return $"Error: {ex.Message}";
        }

        return $"Error: {response.RequestMessage} {response.StatusCode}";
    }
}
