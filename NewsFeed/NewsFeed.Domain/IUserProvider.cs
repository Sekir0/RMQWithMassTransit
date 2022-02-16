using System.Threading.Tasks;

namespace NewsFeed.Domain
{
    public interface IUserProvider
    {
        Task<UserInfo> GetByIdAsync(string id);
    }
}