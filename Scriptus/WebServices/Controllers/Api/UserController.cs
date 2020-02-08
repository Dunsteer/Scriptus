using BaseLogic.Abstractions;
using BaseLogic.Services;
using Commons.Models.User;
using DbServices.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebServices.Helpers;

namespace WebServices.Controllers.Api
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : BaseRESTController<User, UserSearchModel>
    {
        private readonly UserService _userService;
        private readonly IOptions<AppSettings> _appSettings;

        public UserController(BaseService<User> userService, MapperService mapper, IOptions<AppSettings> appSettings, ILogger<UserController> logger) : base(userService, mapper, logger)
        {
            // BaseService<AccessToken> accessTokenService, MapperService mapper, ILogger<AccessToken> logger): base(accessTokenService, mapper, logger)
            _userService = (UserService)userService;
            _appSettings = appSettings;

            _REST.GET.MapTo = typeof(UserViewModel);
            _REST.GET.MapToMin = typeof(UserMinModel);
            _REST.POST.MapTo = typeof(UserViewModel);
            _REST.POST.MapToMin = typeof(UserMinModel);
            _REST.PATCH.MapTo = typeof(UserViewModel);
            _REST.PATCH.MapToMin = typeof(UserMinModel);
            _REST.DELETE.MapTo = typeof(UserViewModel);
            _REST.DELETE.MapToMin = typeof(UserMinModel);

            _REST.PUT.Allowed = false;
            _REST.DELETE.Allowed = false;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]UserAuthModel auth)
        {
            try
            {
                var response = await _userService.Authenticate(auth.Username, auth.Password, _appSettings.Value.Secret);
                if (response == null) return BadRequest();
                else return Ok(response);
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError($"@UserController.Authenticate: {message}");
                return BadRequest(message);
            }
        }

        [HttpGet("microsoft-auth")]
        [AllowAnonymous]
        public async Task<IActionResult> MicrosoftAuthenticate()
        {
            return new ChallengeResult(Microsoft.AspNetCore.Authentication.MicrosoftAccount.MicrosoftAccountDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("HandleMicrosoftLogin", new { returnUrl = "123" })
            });
        }

        [AllowAnonymous]
        [HttpGet("microsoft-callback")]
        public async Task<IActionResult> HandleMicrosoftLogin(string returnUrl = null)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(Microsoft.AspNetCore.Authentication.MicrosoftAccount.MicrosoftAccountDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest();

            var claims = authenticateResult.Principal.Claims;

            var id = Guid.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var fullname = claims.First(x => x.Type == ClaimTypes.Name).Value;
            var email = claims.First(x => x.Type == ClaimTypes.Email).Value;

            var res = await _userService.AuthenticateExternal(id, fullname, email, _appSettings.Value.Secret);

            Response.Cookies.Append("token", res.Token,new Microsoft.AspNetCore.Http.CookieOptions { Expires=DateTime.UtcNow.AddDays(7)});

            return Redirect(Url.Content("~/"));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserRegisterModel user)
        {
            try
            {
                var response = await _userService.Create(new DbServices.Models.User
                {
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    FullName = user.FullName
                });
                if (response == null) return BadRequest();
                else return Ok(response);
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError($"@UserController.Authenticate: {message}");
                return BadRequest(message);
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> WhoAmI()
        {
            try
            {
                var user = await _userService.Get(IdStr);

                if (user == null) return BadRequest();
                else
                {
                    var response = _mapper.Get().Map<UserMinModel>(user);

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError($"@UserController.WhoAmI: {message}");
                return BadRequest(message);
            }
        }
    }
}
