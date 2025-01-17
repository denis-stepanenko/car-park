using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CarPark.Models
{
    public abstract class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = "";
    }
}