using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Services;

namespace SwaggerClone.Pages;

public class IndexModel(ILogger<IndexModel> logger, HttpClient httpClient, ApiAccess apiAccess) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiAccess _apiAccess = apiAccess;

    [BindProperty]
    public string Endpoint { get; set; }
    public string ApiResponse { get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostFetchApiAsync()
    {
        ApiResponse = await _apiAccess.CallApi(Endpoint);
        return Page();
    }
}
