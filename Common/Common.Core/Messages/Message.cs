using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Messages
{
    public abstract class Message
    {
        protected Message()
        {

        }
        [BsonId]
        public string Id { get; set; } = string.Empty;

    }
}
