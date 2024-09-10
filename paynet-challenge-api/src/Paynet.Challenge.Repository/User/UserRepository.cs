using MongoDB.Driver;
using Paynet.Challenge.Entities.User;

namespace Paynet.Challenge.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> _collection;

        public UserRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UserEntity>("User");
        }

        public void Add(UserEntity user)
        {
            _collection.InsertOne(user);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return _collection.Find(user => true).ToList();
        }

        public UserEntity GetByEmail(string email)
        {
            return _collection.Find(user => user.Email == email).FirstOrDefault();
        }

        public void UpdateOneByEmail(string userEmail, UpdateDefinition<UserEntity> update)
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.Email, userEmail);
            _collection.UpdateOne(filter, update);
        }
    }
}
