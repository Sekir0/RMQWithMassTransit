using NewsFeed.Domain;

namespace NewsFeed.Api.Models
{
    public class SearchNewsModel
    {
        public string PublicationId { get; set; }
        
        public int Take { get; set; }
        
        public Ordering order { get; set; }
    }
}