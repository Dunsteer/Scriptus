using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.Post
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public short Type { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
        public string Text { get; set; }
        public List<string> Images { get; set; }
        public short NumberOfQuestions { get; set; }
        public string Pdf { get; set; }
        public List<PostViewModel> Comments { get; set; }
        public short? AnswerFor { get; set; }
        public string UserId { get; set; }
        public List<Guid> VoteUp { get; set; }
        public List<Guid> VoteDown { get; set; }
        public string Name { get; set; }

        public User.UserViewModel User { get; set; }
    }
}
