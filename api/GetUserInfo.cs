using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace Company.Function
{
    public static class GetUserInfo
    {
        [FunctionName("GetUserInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                var claims = ClaimsPrincipal.Current;
                var claimdetails = $"Name:{claims.Identity.Name}, IsAuthenticated:{claims.Identity.IsAuthenticated}";
                var headers = string.Join(',', req.Headers.Select(x => $"{x.Key}:{x.Value}"));

                var date = DateTime.Now.ToString();
                return new JsonResult(new { headers, claimdetails, date });
            }
            catch (Exception exp)
            {
                return new JsonResult(new { exp.Message, exp.StackTrace });
            }
        }
    }
}
