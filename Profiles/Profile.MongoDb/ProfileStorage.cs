using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Profile.Domain;

namespace Profile.MongoDb
{
    public class ProfileStorage : IProfileStorage
    {
        private readonly DbContext _context;

        public ProfileStorage(DbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Profile> FindByIdAsync(Guid id)
        {
            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, id);
            var entity = await _context.Profiles.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }
        
        public async Task<Guid> InsertAsync(Domain.Profile profile)
        {
            var entity = new ProfileEntity
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth?.ToUnixTimeMilliseconds(),
                City = profile.City
            };

            await _context.Profiles.InsertOneAsync(entity);

            return entity.Id;
        }
        
        private static Domain.Profile ToDomain(ProfileEntity entity)
        {
            DateTimeOffset? dateOfBirth = null;
            if (entity.DateOfBirth.HasValue)
            {
                dateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(entity.DateOfBirth.Value);
            }

            return new Domain.Profile(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                dateOfBirth,
                entity.City,
                entity.ProfilePicture);
        }
    }
}