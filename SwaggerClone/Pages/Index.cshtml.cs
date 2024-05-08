using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Services;

namespace SwaggerClone.Pages;

public class IndexModel(ILogger<IndexModel> logger, IApiAccess apiAccess) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IApiAccess _apiAccess = apiAccess;

    [BindProperty] public string Endpoint { get; set; }

    [BindProperty] public bool FormatJson { get; set; }

    public string ApiResponse { get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostFetchApiAsync()
    {
        ApiResponse = await _apiAccess.CallApi(Endpoint, FormatJson);
        return Page();
    }

    public IActionResult OnPostReset()
    {
        ApiResponse = string.Empty;
        return Page();
    }
}
