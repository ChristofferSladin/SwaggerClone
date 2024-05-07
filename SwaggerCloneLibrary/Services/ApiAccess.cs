using SwaggerCloneLibrary.Interfaces;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SwaggerCloneLibrary.Services;

public class ApiAccess(HttpClient httpClient) : IApiAccess
{
    private readonly HttpClient _httpClient = httpClient;



    // THE BOOL IS OT WORKING AS EXPECTED

    public async Task<string> CallApi(string url, bool formatJson)
    {

        if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return "Error: Invalid or missing URL.";
        }

        var response = await _httpClient.GetAsync(url);

        try
        {
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                if (formatJson)
                {
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(responseBody);
                    return JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
                }

                return responseBody;
            }
        }
        catch (HttpRequestException ex)
        {
            return $"Error: {ex.Message}";
        }

        return $"Error: {response.RequestMessage} {response.StatusCode}";
    }
}
