using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SouthernCross.Core.Entities
{
    public class BaseEntity
    {
        [BsonElement("id")]
        [BsonId]
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
