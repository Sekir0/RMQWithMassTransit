using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Profile.Domain
{
    public interface IRelationsService
    {
        Task<IEnumerable<Guid>> GetAllFollowersAsync(Guid userId);

        Task<(IEnumerable<Guid>, long)> SearchFollowersAsync(int skip, int take, Guid userId);

        Task SendRequestAsync(Guid fromUser, Guid toUser);
    }
}