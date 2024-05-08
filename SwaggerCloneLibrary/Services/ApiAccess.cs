using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Utility;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SwaggerCloneLibrary.Services;

public class ApiAccess(HttpClient httpClient) : IApiAccess
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> CallApi(string url, bool formatJson)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        var response = await _httpClient.GetAsync(url);

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
        else return $"Error: {response.RequestMessage} Statuscode: {response.StatusCode}";
    }
}
