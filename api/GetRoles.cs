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
using System.Collections.Generic;
using System.Security.Claims;

namespace Company.Function
{
    public static class GetRoles
    {
        [FunctionName("GetRoles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //string.Join(',',req.Headers.Select(x => $"{x.Key}:{x.Value}"));
            
            string[] _roles = { "admin", "reader"};
            //var claims = identities.Claims.Select(c => $"{c.Type}-{c.Value}").ToList();
            var lst = new List<string>(_roles);
            //lst.AddRange(claims);
            lst.AddRange(req.Headers.Select(x => $"{x.Key}:{x.Value}"));

            var claims = ClaimsPrincipal.Current;
            var claimdetails = $"Name-{claims?.Identity?.Name}-IsAuthenticated-{claims?.Identity?.IsAuthenticated}";

            lst.Add(claimdetails);
            lst.Add("claimdetails");
            var roles = lst.ToArray();
            return new JsonResult(new { roles });
        }
    }
}
