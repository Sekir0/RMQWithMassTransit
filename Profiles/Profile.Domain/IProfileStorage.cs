using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Profile.Domain
{
    public interface IProfileStorage
    {
        Task<Guid> InsertAsync(Profile profile);
        
        Task<Profile> FindByIdAsync(Guid id);

        Task<(IEnumerable<Guid>, long)> SearchProfilesAsync(int skip, int take);
    }
}