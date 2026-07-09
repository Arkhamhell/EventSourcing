using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ticketing.Command.Domain.Common
{
    public class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
    }
}
