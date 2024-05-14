using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwaggerCloneLibrary.Interfaces
{
    public interface IApiAccess
    {
        Task<string> Get(HttpContext httpContext, string url);
        Task<string> GetOne(HttpContext httpContext, string url, int objectId);
        Task<string> DeleteOne(HttpContext httpContext, string endpoint, int objectId);
        Task<string> Post(HttpContext httpContext, string url, string jsonPayload);
        Task<string> Put(HttpContext httpContext, string endpoint, string jsonPayload);
        Task<string> GetJsonTemplate(string url);
    }
}
