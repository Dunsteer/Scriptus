using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.User
{
    public class UserLoginResponseModel : UserMinModel
    {
        public string Token { get; set; }
    }
}
