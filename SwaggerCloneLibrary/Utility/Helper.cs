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

    public static async Task<string> GetSessionIdAsync(string apiKey, string requestToken)
    {
        var client = new HttpClient();
        var content = new StringContent(JsonSerializer.Serialize(new { request_token = requestToken }), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"https://api.themoviedb.org/3/authentication/session/new?api_key={apiKey}", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var sessionResponse = JsonSerializer.Deserialize<SessionResponse>(jsonResponse);
            return sessionResponse.session_id;
        }

        throw new HttpRequestException("Failed to create session ID.");
    }

    public class SessionResponse
    {
        public string session_id { get; set; }
    }

    public class RequestTokenResponse
    {
        public string request_token { get; set; }
    }
}
