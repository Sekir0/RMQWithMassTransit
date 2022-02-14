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

        public Task<Profile> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Profile> CreateAsync(Guid? id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city)
        {
            throw new NotImplementedException();
        }
    }
}