using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Models;
using SwaggerCloneLibrary.Services;
using SwaggerCloneLibrary.Utility;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwaggerClone.Pages
{
    public class AuthModel(IHttpClientFactory httpClientFactory, IApiAuth apiAuth) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IApiAuth _apiAuth = apiAuth;

        [BindProperty] public string AuthUrl { get; set; }
        [BindProperty] public string ErrorMessage { get; set; }
        [BindProperty] public string JsonInput { get; set; }

        public string Token { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAuthenticateAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonInput, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(AuthUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(jsonResponse);
                Token = authResponse?.token;

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    Expires = DateTime.UtcNow.AddHours(1) 
                };
                Response.Cookies.Append("JWTToken", Token, cookieOptions);
            }
            else
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return Page();
        }
    }
}
