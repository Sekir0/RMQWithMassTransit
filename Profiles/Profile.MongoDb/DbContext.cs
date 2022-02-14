using MongoDB.Driver;

namespace Profile.MongoDb
{
    public class DbContext
    {
        private readonly IMongoDatabase _database;

        public DbContext(IMongoDatabase database)
        {
            _database = database;
        }
        
        public IMongoCollection<ProfileEntity> Profiles =>
            _database.GetCollection<ProfileEntity>("profiles");
        
        public IMongoCollection<FriendsEntity> FriendRequests =>
            _database.GetCollection<FriendsEntity>("friendRequests");
    }
}