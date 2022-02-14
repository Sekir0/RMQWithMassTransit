using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsFeed.Domain
{
    public interface IPublicationService
    {
        Task<string> CreateAsync(string text, UserInfo author);
        
        Task<(IEnumerable<Publication>, long, string)> SearchAsync(string publicationId, int take, Ordering order);
    }

    public class PublicationService : IPublicationService
    {
        private readonly IPublicationStorage _publicationStorage;

        public PublicationService(IPublicationStorage publicationStorage)
        {
            _publicationStorage = publicationStorage;
        }

        public async Task<string> CreateAsync(string text, UserInfo author)
        {
            var publication = Publication.New(text, author);

            var id = await _publicationStorage.InsertOneAsync(publication);

            return id;
        }

        public async Task<(IEnumerable<Publication>, long, string)> SearchAsync(string publicationId, int take, Ordering order)
        {
            return await _publicationStorage.FindManyAsync(publicationId, take, order);
        }
    }
}