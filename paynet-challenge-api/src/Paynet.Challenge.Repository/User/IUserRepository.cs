
using MongoDB.Driver;
using Paynet.Challenge.Entities.User;

namespace Paynet.Challenge.Repository
{
    public interface IUserRepository
    {
        void Add(UserEntity user);

        IEnumerable<UserEntity> GetAll();

        UserEntity GetByEmail(string email);

        void UpdateOneByEmail(string userEmail, UpdateDefinition<UserEntity> update);
    }
}