using System;
using System.Net;
using System.Threading.Tasks;
using NewsFeed.Domain;
using NewsFeed.Profiles.HttpClient.Api;
using NewsFeed.Profiles.HttpClient.Client;

namespace NewsFeed.Api.Helpers
{
    public class ProfilesApiUserProvider : IUserProvider
    {
        private readonly IProfileApi _profilesApi;

        public ProfilesApiUserProvider(IProfileApi profilesApi)
        {
            _profilesApi = profilesApi;
        }

        public async Task<UserInfo> GetByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return null;
            }

            try
            {
                var result = _profilesApi.ProfileIdGet(guid);
                return result == null
                    ? null
                    : new UserInfo(result.Id, $"{result.FirstName} {result.LastName}", null);
            }
            catch (ApiException ex) when (ex.ErrorCode == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}