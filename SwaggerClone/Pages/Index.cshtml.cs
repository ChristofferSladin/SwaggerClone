using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SwaggerCloneLibrary.Interfaces;

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
    [BindProperty] public bool UseAuth { get; set; }
    [BindProperty] public string ErrorMessage { get; set; }



    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostExecuteAsync()
    {
        try
        {
            if (RequestType == "GET" && ObjectId > 0)
                ApiResponse = await _apiAccess.GetOne(HttpContext, Endpoint, ObjectId);

            else if (RequestType == "GET")
                ApiResponse = await _apiAccess.Get(HttpContext, Endpoint);

            else if (RequestType == "DELETE")
                ApiResponse = await _apiAccess.DeleteOne(HttpContext, Endpoint, ObjectId);

            ErrorMessage = null;
        }
        catch (UnauthorizedAccessException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = ex.Message;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostGetAsync()
    {
        ApiResponse = await _apiAccess.Get(HttpContext, Endpoint);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        ApiResponse = await _apiAccess.DeleteOne(HttpContext, Endpoint, ObjectId);
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

    //public async Task<IActionResult> OnPostPostAsync(string endpoint, string jsonPayload)
    //{
    //    ApiResponse = await _apiAccess.Post(HttpContext, endpoint, jsonPayload);
    //    return Page();
    //}

    //public async Task<IActionResult> OnPostPutAsync()
    //{
    //    ApiResponse = await _apiAccess.Put(Endpoint, JsonPayload);
    //    return Page();
    //}
}
