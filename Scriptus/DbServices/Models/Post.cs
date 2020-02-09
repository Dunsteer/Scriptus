using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DbServices.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public short Type { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string[] Tags { get; set; }
        public string Text { get; set; }
        public string[] Images { get; set; }
        public short NumberOfQuestions { get; set; }
        public string Pdf { get; set; }
        public List<Post> Comments { get; set; } = new List<Post>();
        public short? AnswerFor { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> VoteUp { get; set; } = new List<Guid>();
        public List<Guid> VoteDown { get; set; } = new List<Guid>();
        public string Name { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}
