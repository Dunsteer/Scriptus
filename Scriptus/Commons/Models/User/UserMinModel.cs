using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.User
{
    public class UserMinModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public short Rank { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> AditionalEmails { get; set; }
    }
}
