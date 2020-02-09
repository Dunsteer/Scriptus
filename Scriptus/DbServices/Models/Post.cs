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
        public DateTime Date { get; set; }
        public string[] Tags { get; set; }
        public string Text { get; set; }
        public string[] Images { get; set; }
        public short NumberOfQuestions { get; set; }
        public string Pdf { get; set; }
        public Post[] Comments { get; set; }
        public short? AnswerFor { get; set; }
        public Guid UserId { get; set; }
        public Guid[] VoteUp { get; set; }
        public Guid[] VoteDown { get; set; }
        public string Name { get; set; }
        
        [NotMapped]
        public User User { get; set; }
    }
}
