using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ticketing.Command.Domain.Common
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
    }
}
