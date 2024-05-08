using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerCloneLibrary.Interfaces
{
    public interface IApiAccess
    {
        Task<string> Get(string url, bool formatJson);
        Task<string> Delete(string endpoint);
        Task<string> Post(string endpoint, string jsonPayload);
        Task<string> Put(string endpoint, string jsonPayload);
    }
}
