using Commons.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.User
{
    public class UserSearchModel : PagerBase
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
