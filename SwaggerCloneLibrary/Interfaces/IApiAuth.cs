namespace SwaggerCloneLibrary.Interfaces;

public interface IApiAuth
{
    Task<string> Get(string url, string token);
    Task<string> CreateRequestTokenAsync(string url, string apiKey);
    Task<string> GetSessionIdAsync(string url, string apiKey, string requestToken);
}
