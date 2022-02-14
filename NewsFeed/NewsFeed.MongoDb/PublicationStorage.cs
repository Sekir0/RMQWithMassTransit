using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NewsFeed.Domain;

namespace NewsFeed.MongoDb
{
    public class PublicationStorage : IPublicationStorage
    {
        private readonly DbContext _context;

        public PublicationStorage(DbContext context)
        {
            _context = context;
        }
        
        public async Task<string> InsertOneAsync(Publication publication)
        {
            var entity = new PublicationEntity
            {
                Content = publication.Content,
                Author = (UserInfoEntity)publication.Author,
                CreateOn = publication.CreatedOn.ToUnixTimeMilliseconds()
            };

            await _context.Publications.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public async Task<(IEnumerable<Publication>, long, string)> FindManyAsync(string publicationId, int take, Ordering order)
        {
            var filter = order switch
            {
                Ordering.Desc => Builders<PublicationEntity>.Filter.Lt(x => x.Id, ObjectId.Parse(publicationId)),
                _ => Builders<PublicationEntity>.Filter.Gt(x => x.Id, ObjectId.Parse(publicationId))
            };

            var totalCount = await _context.Publications.Find(filter)
                .CountDocumentsAsync();

            var sorting = order switch
            {
                Ordering.Desc => Builders<PublicationEntity>.Sort.Descending(x => x.CreateOn),
                _ => Builders<PublicationEntity>.Sort.Ascending(x => x.CreateOn)
            };

            var entities = await _context.Publications.Find(filter)
                .Sort(sorting)
                .Limit(take)
                .ToListAsync();

            return (entities.Select(ToDomain), totalCount, entities.Select(x => x.Id).Last().ToString());
        }
        
        private static Publication ToDomain(PublicationEntity entity)
        {
            return new Publication(
                entity.Id.ToString(),
                entity.Content,
                (UserInfo) entity.Author,
                DateTimeOffset.FromUnixTimeMilliseconds(entity.CreateOn));
        }
    }
}