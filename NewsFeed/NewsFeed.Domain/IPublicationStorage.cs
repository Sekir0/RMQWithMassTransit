using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsFeed.Domain
{
    public interface IPublicationStorage
    {
        Task<string> InsertOneAsync(Publication publication);
        
        Task<(IEnumerable<Publication>, long, string)> FindManyAsync(string publicationId, int take, Ordering order);
    }
}