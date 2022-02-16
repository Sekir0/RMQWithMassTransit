using System;
using System.Threading.Tasks;

namespace Profile.Domain
{
    public interface IProfileService
    {
        Task<Profile> GetByIdAsync(Guid id);
        
        Task<Profile> CreateAsync(Guid? id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileStorage _profileStorage;

        public ProfileService(IProfileStorage profileStorage)
        {
            _profileStorage = profileStorage;
        }

        public async Task<Profile> GetByIdAsync(Guid id)
        {
            return await _profileStorage.FindByIdAsync(id);
        }

        public async Task<Profile> CreateAsync(Guid? id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city)
        {
            var profile = new Profile(id ?? Guid.NewGuid(), firstName, lastName, gender, dateOfBirth, city);

            await _profileStorage.InsertAsync(profile);

            return profile;
        }
    }
}