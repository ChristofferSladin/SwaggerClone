namespace SwaggerCloneLibrary.Interfaces;

public interface IApiAuth
{
    Task<string> Get(string url, string token);
}
