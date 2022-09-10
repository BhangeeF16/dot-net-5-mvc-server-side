using Common.ViewModels.UserAccount;

namespace Domain.IServices
{
    public interface ICurrentUserService
    {
        string Fullname { get; }
        public string UserId { get; }
        UserVM User { get; }
        public string BaseUrl { get; }
    }
}
