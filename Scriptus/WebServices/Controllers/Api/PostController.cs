using BaseLogic.Abstractions;
using BaseLogic.Services;
using Commons.Models.Post;
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
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostController : BaseRESTController<Post, PostSearchModel>
    {
        private readonly PostService _postService;
        private readonly IOptions<AppSettings> _appSettings;

        public PostController(BaseService<Post> postService, MapperService mapper, IOptions<AppSettings> appSettings, ILogger<PostController> logger) : base(postService, mapper, logger)
        {
            // BaseService<AccessToken> accessTokenService, MapperService mapper, ILogger<AccessToken> logger): base(accessTokenService, mapper, logger)
            _postService = (PostService)postService;
            _appSettings = appSettings;

            _REST.GET.MapTo = typeof(PostViewModel);
            _REST.GET.MapToMin = typeof(PostMinModel);
            _REST.POST.MapTo = typeof(PostViewModel);
            _REST.POST.MapToMin = typeof(PostMinModel);
            _REST.PATCH.MapTo = typeof(PostViewModel);
            _REST.PATCH.MapToMin = typeof(PostMinModel);
            _REST.DELETE.MapTo = typeof(PostViewModel);
            _REST.DELETE.MapToMin = typeof(PostMinModel);

            _REST.PUT.Allowed = false;
            _REST.DELETE.Allowed = false;
        }
    }
}
