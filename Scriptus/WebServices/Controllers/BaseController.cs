using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string IdStr => User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        protected string Username => User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
        protected bool LoggedIn => User.Claims.Any();
    }
}
