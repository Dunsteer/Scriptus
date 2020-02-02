using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbServices.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public short Rank { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> AditionalEmails { get; set; }
    }
}
