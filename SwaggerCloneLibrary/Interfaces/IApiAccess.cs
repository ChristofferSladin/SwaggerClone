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
        Task<string> Get(string url);
        Task<string> GetOne(string url, int objectId);
        Task<string> DeleteOne(string endpoint, int objectId);
        Task<string> Post(string endpoint, string jsonPayload);
        Task<string> Put(string endpoint, string jsonPayload);
        Task<string> GetJsonTemplate(string url);
    }
}
