using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Profile.Domain;

namespace Profile.MongoDb
{
    public class RelationsStorage : IRelationsService
    {
        private readonly DbContext _context;

        public RelationsStorage(DbContext context)
        {
            _context = context;
        }
        
        private static FilterDefinitionBuilder<FriendsEntity> Filter => Builders<FriendsEntity>.Filter;

        public async Task<IEnumerable<Guid>> GetAllFollowersAsync(Guid userId)
        {
            var filter = Filter.Eq(p => p.ToUser, userId)
                         & (Filter.Eq(p => p.Status, RequestStatus.Declined) |
                            Filter.Eq(p => p.Status, RequestStatus.Incoming));

            var requestCount = await _context.FriendRequests
                .Find(filter)
                .CountDocumentsAsync();

            var requests = await _context.FriendRequests
                .Find(filter)
                .ToListAsync();

            return requests.Select(r => r.FromUser).ToList();
        }

        public async Task<(IEnumerable<Guid>, long)> SearchFollowersAsync(int skip, int take, Guid userId)
        {
            var filter = Filter.Eq(p => p.ToUser, userId)
                         & (Filter.Eq(p => p.Status, RequestStatus.Declined) |
                            Filter.Eq(p => p.Status, RequestStatus.Incoming));

            var requestCount = await _context.FriendRequests
                .Find(filter)
                .CountDocumentsAsync();

            var requests = await _context.FriendRequests
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return (requests.Select(r => r.FromUser).ToList(), requestCount);
        }

        public async Task SendRequestAsync(Guid fromUser, Guid toUser)
        {
            var filter = Filter.Eq(p => p.FromUser, fromUser) & Filter.Eq(p => p.ToUser, toUser);

            var request = await _context.FriendRequests
                .Find(filter)
                .FirstOrDefaultAsync();

            if (request == null)
            {
                await _context.FriendRequests.InsertManyAsync(new[]
                {
                    new FriendsEntity { FromUser = fromUser, ToUser = toUser, Status = RequestStatus.Incoming },
                    new FriendsEntity { FromUser = toUser, ToUser = fromUser, Status = RequestStatus.Outgoing }
                });
            }
        }
    }
}