using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Interfaces;
using SwaggerCloneLibrary.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SwaggerClone.Pages;

public class IndexModel(ILogger<IndexModel> logger, IApiAccess apiAccess) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IApiAccess _apiAccess = apiAccess;

    [BindProperty] public string Endpoint { get; set; }
    [BindProperty] public string JsonPayload { get; set; }
    [BindProperty] public int ObjectId { get; set; }
    [BindProperty] public string ApiResponse { get; set; }
    [BindProperty] public string RequestType { get; set; }

    public void OnGet()
    {

    }
    public async Task<IActionResult> OnPostExecuteAsync()
    {
        if (RequestType == "GET")
            ApiResponse = await _apiAccess.GetOne(Endpoint, ObjectId);

        else if (RequestType == "DELETE")
            ApiResponse = await _apiAccess.DeleteOne(Endpoint, ObjectId);

        return Page();
    }

    public async Task<IActionResult> OnPostGetAsync()
    {
        ApiResponse = await _apiAccess.Get(Endpoint);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        ApiResponse = await _apiAccess.DeleteOne(Endpoint, ObjectId);
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
        ObjectId = 0;
        JsonPayload = string.Empty;
        RequestType = "GET";
        Endpoint = string.Empty;
        return Page();
    }

    //public async Task<IActionResult> OnPostPostAsync()
    //{
    //    ApiResponse = await _apiAccess.Post(Endpoint, JsonPayload);
    //    return Page();
    //}

    //public async Task<IActionResult> OnPostPutAsync()
    //{
    //    ApiResponse = await _apiAccess.Put(Endpoint, JsonPayload);
    //    return Page();
    //}
}
