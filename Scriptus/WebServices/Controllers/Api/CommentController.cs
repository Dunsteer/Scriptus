using BaseLogic.Abstractions;
using BaseLogic.Services;
using Commons.Models.Comment;
using DbServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Helpers;

namespace WebServices.Controllers.Api
{
    [Route("api/comments")]
    [ApiController]
    [Authorize]
    public class CommentController : BaseRESTController<Comment, CommentSearchModel>
    {
        private readonly CommentService _commentService;
        private readonly IOptions<AppSettings> _appSettings;

        public CommentController(BaseService<Comment> commentService, MapperService mapper, IOptions<AppSettings> appSettings, ILogger<PostController> logger) : base(commentService, mapper, logger)
        {
            // BaseService<AccessToken> accessTokenService, MapperService mapper, ILogger<AccessToken> logger): base(accessTokenService, mapper, logger)
            _commentService = (CommentService)commentService;
            _appSettings = appSettings;

            _REST.GET.MapTo = typeof(CommentViewModel);
            _REST.GET.MapToMin = typeof(CommentMinModel);
            _REST.POST.MapTo = typeof(CommentViewModel);
            _REST.POST.MapToMin = typeof(CommentMinModel);
            _REST.PATCH.MapTo = typeof(CommentViewModel);
            _REST.PATCH.MapToMin = typeof(CommentMinModel);
            _REST.DELETE.MapTo = typeof(CommentViewModel);
            _REST.DELETE.MapToMin = typeof(CommentMinModel);

            _REST.PUT.Allowed = false;
            _REST.DELETE.Allowed = false;
        }
    }
}
