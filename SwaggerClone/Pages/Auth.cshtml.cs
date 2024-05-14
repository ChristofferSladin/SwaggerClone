using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Models;
using SwaggerCloneLibrary.Utility;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwaggerClone.Pages
{
    public class AuthModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string AuthUrl { get; set; }
        [BindProperty] public string TESTING { get; set; }
        [BindProperty] public string RequestTokenUrl { get; set; }
        [BindProperty] public string ApiKey { get; set; }
        [BindProperty] public string ShortUrl { get; set; }

        [BindProperty] public string SessionId { get; set; }
        [BindProperty] public string Something { get; set; }

        [BindProperty] public string RequestToken { get; set; }
        [BindProperty] public string ErrorMessage { get; set; }



        public string Token { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostGetSomethingAsync()
        {
            try
            {
                Something = await Helper.GetAsync(ShortUrl, ApiKey, SessionId);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreateRequestTokenAsync()
        {
            SessionId = await Helper.CreateRequestTokenAsync(RequestTokenUrl, ApiKey);
            return Page();
        }


       


        public async Task<IActionResult> OnPostAuthenticateAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var client = _httpClientFactory.CreateClient();
            var authRequest = new
            {
                email = Email,
                password = Password
            };

            var content = new StringContent(JsonSerializer.Serialize(authRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(AuthUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(jsonResponse);
                Token = authResponse.access_token;

                // Set the token in a cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure this is true in production to use HTTPS
                    Expires = DateTime.UtcNow.AddHours(1) // Set expiration as needed
                };
                Response.Cookies.Append("JWTToken", Token, cookieOptions);
            }
            else
            {
                TESTING = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return Page();
        }
    }
}
