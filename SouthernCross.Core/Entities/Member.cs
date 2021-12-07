using MongoDB.Bson.Serialization.Attributes;
using SouthernCross.Core.Helper;
using System;
using System.Text.Json.Serialization;

namespace SouthernCross.Core.Entities
{
    public class Member : BaseEntity
    {
        [BsonElement("firstName")]
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [BsonElement("memberCardNumber")]
        [JsonPropertyName("memberCardNumber")]
        public string MemberCardNumber { get; set; }

        [BsonElement("policyNumber")]
        [JsonPropertyName("policyNumber")]
        public string PolicyNumber { get; set; }

        [BsonElement("dateOfBirth")]
        [JsonPropertyName("dataOfBirth")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DateOfBirth { get; set; }
    }

}
