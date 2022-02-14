using MongoDB.Driver;

namespace NewsFeed.MongoDb
{
    public class DbContext
    {
        private readonly IMongoDatabase database;

        public DbContext(IMongoDatabase database)
        {
            this.database = database;
        }
        
        public IMongoCollection<PublicationEntity> Publications =>
            database.GetCollection<PublicationEntity>("publications");
    }
}