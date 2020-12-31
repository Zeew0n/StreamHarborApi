using System;
using System.Linq;
using WorkFlowTaskManager.Application.Services.CurrentUserService;
using Microsoft.AspNetCore.Http;
using WorkFlowTaskManager.Infrastructure.Identity.Helpers;

namespace WorkFlowTaskManager.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _context;

        public CurrentUserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetUser()
        {
            var userId = _context.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == AuthConstants.JwtId)?.Value;
            return userId == null ? Guid.Empty : Guid.Parse(userId);
        }
    }
}