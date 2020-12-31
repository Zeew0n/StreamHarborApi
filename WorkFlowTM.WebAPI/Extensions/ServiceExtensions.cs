using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkFlowTaskManager.Application.Services.CurrentUserService;
using WorkFlowTaskManager.Services;

namespace WorkFlowTaskManager.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddWbApiLayer(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
        }
    }
}