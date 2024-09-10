using MongoDbGenericRepository.Models;

namespace Paynet.Challenge.Entities
{
    public class BaseEntity : Document
    {
        public DateTime? LastUpdateDateUtc { get; set; }
    }
}