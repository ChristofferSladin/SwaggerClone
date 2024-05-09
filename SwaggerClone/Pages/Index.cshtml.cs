using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SwaggerClone.Pages;

public class IndexModel(ILogger<IndexModel> logger, IApiAccess apiAccess) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IApiAccess _apiAccess = apiAccess;

    [BindProperty] public string Endpoint { get; set; }
    [BindProperty] public string JsonPayload { get; set; }
    [BindProperty] public int ObjectId { get; set; }
    [BindProperty] public string ApiResponse { get; set; }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostGetAsync()
    {
        ApiResponse = await _apiAccess.Get(Endpoint);
        return Page();
    }

    public async Task<IActionResult> OnPostPostAsync()
    {
        ApiResponse = await _apiAccess.Post(Endpoint, JsonPayload);
        return Page();
    }

    public async Task<IActionResult> OnPostPutAsync()
    {
        ApiResponse = await _apiAccess.Put(Endpoint, JsonPayload);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        ApiResponse = await _apiAccess.Delete(Endpoint, ObjectId);
        return Page();
    }

    public async Task<IActionResult> OnPostFetchJsonTemplateAsync()
    {
        JsonPayload = await _apiAccess.GetJsonTemplate(Endpoint);
        return Page();
    }

    public IActionResult OnPostReset()
    {
        ApiResponse = string.Empty;
        return Page();
    }
}
