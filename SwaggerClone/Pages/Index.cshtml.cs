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
    [BindProperty] public int PostObjectId { get; set; }
    [BindProperty] public int PutObjectId { get; set; }
    [BindProperty] public string GetResponse { get; set; }
    [BindProperty] public string DeleteResponse { get; set; }
    [BindProperty] public string PostResponse { get; set; }
    [BindProperty] public string PostRequestBody { get; set; }
    [BindProperty] public string PutResponse { get; set; }
    [BindProperty] public string PutRequestBody { get; set; }
    [BindProperty] public string RequestType { get; set; }
    [BindProperty] public string ErrorMessage { get; set; }


    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostFetchJsonTemplateAsync()
    {
        JsonPayload = await _apiAccess.GetJsonTemplate(Endpoint);
        return Page();
    }

    public async Task<IActionResult> OnPostGetAsync()
    {
        GetResponse = await _apiAccess.Get(HttpContext, Endpoint);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        DeleteResponse = await _apiAccess.DeleteOne(HttpContext, Endpoint, ObjectId);
        return Page();
    }

    public async Task<IActionResult> OnPostPostAsync()
    {
        PostResponse = await _apiAccess.Post(HttpContext, Endpoint, PostRequestBody);
        return Page();
    }
    public async Task<IActionResult> OnPostPutAsync()
    {
        PutResponse = await _apiAccess.Put(HttpContext, Endpoint, PutRequestBody);
        return Page();
    }


    public IActionResult OnPostReset()
    {
        GetResponse = string.Empty;
        DeleteResponse = string.Empty;
        ObjectId = 0;
        JsonPayload = string.Empty;
        RequestType = "GET";
        Endpoint = string.Empty;
        return Page();
    }
}
