using System;
using System.Threading.Tasks;

namespace Profile.Domain
{
    public interface IProfileStorage
    {
        Task<Guid> InsertAsync(Profile profile);
        
        Task<Profile> FindByIdAsync(Guid id);
    }
}