using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.User
{
    public class UserRegisterModel: UserAuthModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }

    }
}
