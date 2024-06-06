using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoUzduotis.Models
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}
