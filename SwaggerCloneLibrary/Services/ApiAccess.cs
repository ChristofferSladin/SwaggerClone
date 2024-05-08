using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Utility;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SwaggerCloneLibrary.Services;

public class ApiAccess(HttpClient httpClient) : IApiAccess
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> Get(string url, bool formatJson)
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

    // POST Method
    public async Task<string> Post(string url, string jsonPayload)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    // PUT Method
    public async Task<string> Put(string url, string jsonPayload)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE Method
    public async Task<string> Delete(string url)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        var response = await _httpClient.DeleteAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}
