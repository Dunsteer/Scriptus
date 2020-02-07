using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbServices.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParentId { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Data { get; set; }
        public List<Comment> Replies { get; set; }
    }
}
