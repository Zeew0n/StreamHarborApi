using WorkFlowTaskManager.Infrastructure.Identity.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

using Serilog;

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkFlowTaskManager.WebAPI.Attributes
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        private string _actionName = string.Empty;

        public PermissionAttribute(string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(_actionName, claimValue) };
        }

        public class ClaimRequirementFilter : IAsyncActionFilter
        {
            private readonly Claim _claim;

            public ClaimRequirementFilter(Claim claim)
            {
                _claim = claim;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var user = context.HttpContext.User;
                string admin = user.FindFirst(AuthConstants.IsAdmin).Value;
                bool isAdmin = admin.ToLower() == "true";
                if (isAdmin)
                {
                    await next();
                    return;
                }

                Claim permissionClaim = context.HttpContext.User.FindFirst(AuthConstants.Permissions);
                if (permissionClaim != null && !string.IsNullOrEmpty(permissionClaim.Value))
                {
                    try
                    {
                        var userPermissions = JsonConvert.DeserializeObject<List<string>>(permissionClaim.Value);
                        if (userPermissions.Contains(_claim.Value))
                        {
                            await next();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                    }
                }

                HttpStatusCode forbidden = HttpStatusCode.Forbidden;
                int forbiddenValue = (int)forbidden;
                Log.Error("Error: {Code},{Status}", forbiddenValue, forbidden);
                context.HttpContext.Response.StatusCode = forbiddenValue;
                return;
            }
        }
    }

    public static class CheckPermissionExtension
    {
        public static bool CheckPermission(this ClaimsPrincipal user, string claimPermission)
        {
            Claim permissionClaim = user.FindFirst("permission");
            if (permissionClaim != null && !string.IsNullOrEmpty(permissionClaim.Value))
            {
                try
                {
                    var userPermissions = JsonConvert.DeserializeObject<List<string>>(permissionClaim.Value);
                    if (userPermissions.Contains(claimPermission))
                    {
                        return true;
                    }
                }
                catch
                { }
            }
            return false;
        }
    }
}