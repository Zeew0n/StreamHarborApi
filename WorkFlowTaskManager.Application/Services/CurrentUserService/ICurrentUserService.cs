using System;

namespace WorkFlowTaskManager.Application.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        Guid GetUser();
    }
}