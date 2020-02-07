using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.Post
{
    public class PostMinModel
    {
        public string Id { get; set; }
        public short Type { get; set; }
        public DateTime Date { get; set; }
        public string[] Tags { get; set; }
        public string Text { get; set; }
        public string[] Images { get; set; }
        public short NumberOfQuestions { get; set; }
        public string Pdf { get; set; }
        public PostViewModel[] Comments { get; set; }
        public short? AnswerFor { get; set; }
        public string UserId { get; set; }
        public string[] VoteUp { get; set; }
        public string[] VoteDown { get; set; }
    }
}
