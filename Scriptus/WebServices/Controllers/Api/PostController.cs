using BaseLogic.Abstractions;
using BaseLogic.Services;
using Commons.Models.Post;
using DbServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly UserService _userService;
        private readonly IOptions<AppSettings> _appSettings;

        public PostController(BaseService<User> userService, BaseService<Post> postService, MapperService mapper, IOptions<AppSettings> appSettings, ILogger<PostController> logger) : base(postService, mapper, logger)
        {
            // BaseService<AccessToken> accessTokenService, MapperService mapper, ILogger<AccessToken> logger): base(accessTokenService, mapper, logger)
            _postService = (PostService)postService;
            _userService = (UserService)userService;
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
        }

        [AllowAnonymous]
        public override Task<IActionResult> GetAll([FromQuery] PostSearchModel search, [FromQuery] bool min = false)
        {
            return base.GetAll(search, min);
        }

        [AllowAnonymous]
        public override Task<IActionResult> Get(Guid id, [FromQuery] bool min = false)
        {
            return base.Get(id, min);
        }

        [HttpPost("{id}/vote-up")]
        public async Task<IActionResult> VoteUp(Guid id)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                PostVoteUp(post, UserId);

                await _postService.Update(id,post);
            }

            var type = _REST.GET.MapTo;
            if (type == null) return Ok(post);

            return Ok(_mapper.Get().Map(post, typeof(Post), type));
        }

        [HttpPost("{id}/vote-down")]
        public async Task<IActionResult> VoteDown(Guid id)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                PostVoteDown(post, UserId);

                await _postService.Update(id, post);
            }

            var type = _REST.GET.MapTo;
            if (type == null) return Ok(post);

            return Ok(_mapper.Get().Map(post, typeof(Post), type));
        }

        public override async Task<IActionResult> Delete(Guid id, [FromQuery] bool min = true)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                var user = await _userService.Get(this.UserId);
                if (post.UserId == this.UserId || user.Rank == 1)
                {
                    return await base.Delete(id, min);
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            List<string> paths = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(_appSettings.Value.UploadPath, Path.GetRandomFileName());

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    paths.Add(filePath);
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, paths });
        }

        [HttpPost("{id}/vote-up/{commentId}")]
        public async Task<IActionResult> CommentVoteUp(Guid id,Guid commentId)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                var comment = post.Comments.FirstOrDefault(x => x.Id == commentId);
                if (comment != null)
                {
                    PostVoteUp(post, UserId);

                    await _postService.Update(id, post);

                    post = comment;
                }
            }

            var type = _REST.GET.MapTo;
            if (type == null) return Ok(post);

            return Ok(_mapper.Get().Map(post, typeof(Post), type));
        }

        [HttpPost("{id}/vote-down/{commentId}")]
        public async Task<IActionResult> CommentVoteDown(Guid id, Guid commentId)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                var comment = post.Comments.FirstOrDefault(x => x.Id == commentId);
                if (comment != null)
                {
                    PostVoteDown(comment, UserId);

                    await _postService.Update(id, post);

                    post = comment;
                }  
            }

            var type = _REST.GET.MapTo;
            if (type == null) return Ok(post);

            return Ok(_mapper.Get().Map(post, typeof(Post), type));
        }

        private void PostVoteUp(Post post, Guid UserId)
        {
            if (post != null)
            {
                if (post.VoteDown != null && !post.VoteDown.Contains(UserId))
                {
                    post.VoteDown.Add(UserId);
                }
                else
                {
                    if (post.VoteDown != null)
                    {
                        post.VoteDown.Remove(UserId);
                    }
                }

                if (post.VoteUp != null && post.VoteUp.Contains(UserId))
                {
                    post.VoteUp.Remove(UserId);
                }
            }
        }

        private void PostVoteDown(Post post, Guid UserId)
        {
            if (post != null)
            {
                if (post.VoteDown != null && !post.VoteDown.Contains(UserId))
                {
                    post.VoteDown.Add(UserId);
                }
                else
                {
                    if (post.VoteDown != null)
                    {
                        post.VoteDown.Remove(UserId);
                    }
                }

                if (post.VoteUp != null && post.VoteUp.Contains(UserId))
                {
                    post.VoteUp.Remove(UserId);
                }
            }
        }
    }
}
