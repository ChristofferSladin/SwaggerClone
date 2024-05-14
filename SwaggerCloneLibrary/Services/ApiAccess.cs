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

    public async Task<string> Get(string url)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
            return Helper.FormatJson(await response.Content.ReadAsStringAsync());

        else return $"Error: {response.RequestMessage} Statuscode: {response.StatusCode}";
    }

    public async Task<string> GetOne(string url, int objectId)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        if (objectId > 0)
            url = $"{url}/{objectId}";

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
            return Helper.FormatJson(await response.Content.ReadAsStringAsync());

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

    public async Task<string> DeleteOne(string url, int objectId)
    {
        if (Validation.IsNotValidUrl(url))
            return "Error: URL not valid";

        if (Validation.IsNotWellFormedUrl(url))
            return "Error: URL not formed properly";

        if (objectId <= 0)
            return $"Error: The object does not exist";

        try
        {
            string fullUrl = $"{url.TrimEnd('/')}/{objectId}";

            var deletedObject = await GetOne(url, objectId);
            var response = await _httpClient.DeleteAsync(fullUrl);

            if (response.IsSuccessStatusCode)
                return $"Object successfully deleted:\n\n{deletedObject}";

            else
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> GetJsonTemplate(string url)
    {
        try
        {
            if (Validation.IsNotValidUrl(url))
                return "Error: URL not valid";

            if (Validation.IsNotWellFormedUrl(url))
                return "Error: URL not formed properly";

            var response = await _httpClient.GetStringAsync(url);
            var jsonDocument = JsonDocument.Parse(response);
            var rootElement = jsonDocument.RootElement;

            var jsonObject = rootElement.ValueKind == JsonValueKind.Array ? rootElement[0] : rootElement;

            var modifiedJson = ReplaceWithDefaults(jsonObject);

            return JsonSerializer.Serialize(modifiedJson, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    private static Dictionary<string, object> ReplaceWithDefaults(JsonElement element)
    {
        var result = new Dictionary<string, object>();

        foreach (var property in element.EnumerateObject())
        {
            switch (property.Value.ValueKind)
            {
                case JsonValueKind.String:
                    result[property.Name] = string.Empty;
                    break;
                case JsonValueKind.Number:
                    result[property.Name] = 0;
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    result[property.Name] = false;
                    break;
                case JsonValueKind.Array:
                    result[property.Name] = new List<object>();
                    break;
                case JsonValueKind.Object:
                    result[property.Name] = ReplaceWithDefaults(property.Value);
                    break;
                default:
                    result[property.Name] = null;
                    break;
            }
        }
        return result;
    }
}
