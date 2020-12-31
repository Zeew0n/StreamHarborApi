using System;

namespace WorkFlowTaskManager.Domain.Models.AppUserModels
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsExpired { get; private set; }
        public Guid UserId { get; private set; }
        public AppUser AppUser { get; set; }

        public RefreshToken(string token, Guid userId)
        {
            Token = token;
            ExpiryDate = DateTime.Now.AddDays(7);
            UserId = userId;
        }

        public static RefreshToken CreateRefreshToken(string token, Guid userId) => new RefreshToken(token, userId);

        public void UpdateToken(string token)
        {
            Token = token;
            ExpiryDate = DateTime.Now.AddDays(10);
        }
    }
}