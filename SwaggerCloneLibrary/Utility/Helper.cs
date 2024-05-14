using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwaggerCloneLibrary.Utility;

public class Helper
{
    public static string FormatJson(string json)
    {
        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            return JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            return json;
        }
    }

    public static async Task<string> CreateRequestTokenAsync(string url, string apiKey)
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

    public static async Task<string> GetAsync(string url, string apiKey, string sessionId)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"{url}?api_key={apiKey}&session_id={sessionId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new HttpRequestException("Failed to get account details.");
    }

    public class RequestTokenResponse
    {
        public string request_token { get; set; }
    }
}
