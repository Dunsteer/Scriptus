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
                await PostVoteUp(post, UserId);

                await _postService.Update(id, post);
            }

            return Map(post, false);
        }

        [HttpPost("{id}/vote-down")]
        public async Task<IActionResult> VoteDown(Guid id)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                await PostVoteDown(post, UserId);

                await _postService.Update(id, post);
            }

            return Map(post, false);
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

        List<string> fileExtensions = new List<string>() { ".png", ".jpg", ".jpeg", ".gif", ".pdf" };

        [HttpPost("file-upload")]
        public async Task<IActionResult> UploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            List<string> paths = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0 && fileExtensions.Contains(Path.GetExtension(formFile.FileName)))
                {
                    var filePath = Path.Combine(_appSettings.Value.UploadPath, $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(formFile.FileName)}");

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    paths.Add(filePath.Replace("/wwwroot", ""));
                }
            }

            return Ok(new { count = files.Count, size, paths });
        }

        [HttpPost("{id}/vote-up/{commentId}")]
        public async Task<IActionResult> CommentVoteUp(Guid id, Guid commentId)
        {
            var post = await _postService.Get(id);

            if (post != null)
            {
                var comment = post.Comments.FirstOrDefault(x => x.Id == commentId);
                if (comment != null)
                {
                    await PostVoteUp(comment, UserId);

                    await _postService.Update(id, post);

                    post = comment;
                }
            }

            return Map(post, false);
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
                    await PostVoteDown(comment, UserId);

                    await _postService.Update(id, post);

                    post = comment;
                }
            }

            return Map(post, false);
        }

        private async Task PostVoteUp(Post post, Guid UserId)
        {
            if (post != null)
            {
                var user = await _userService.Get(UserId);

                if (post.VoteUp != null && !post.VoteUp.Contains(UserId))
                {
                    post.VoteUp.Add(UserId);
                    user.Reputation++;
                }
                else
                {
                    if (post.VoteUp != null)
                    {
                        post.VoteUp.Remove(UserId);
                        user.Reputation--;
                    }
                }

                if (post.VoteDown != null && post.VoteDown.Contains(UserId))
                {
                    post.VoteDown.Remove(UserId);
                    user.Reputation++;
                }

                await _userService.Update(UserId,user);
            }
        }

        private async Task PostVoteDown(Post post, Guid UserId)
        {
            if (post != null)
            {
                var user = await _userService.Get(UserId);
                if (post.VoteDown != null && !post.VoteDown.Contains(UserId))
                {
                    post.VoteDown.Add(UserId);
                    user.Reputation--;
                }
                else
                {
                    if (post.VoteDown != null)
                    {
                        post.VoteDown.Remove(UserId);
                        user.Reputation++;
                    }
                }

                if (post.VoteUp != null && post.VoteUp.Contains(UserId))
                {
                    post.VoteUp.Remove(UserId);
                    user.Reputation--;
                }
            }
        }

        public override Task<IActionResult> Create([FromBody] Post model, [FromQuery] bool min = false)
        {
            model.UserId = UserId;

            if (model.Type == 1)
            {
                model.Tags.Insert(0, "ispit");
            }

            if (model.Type == 2)
            {
                model.Tags.Insert(0, "kolokvijum");
            }

            return base.Create(model, min);
        }

        [HttpPost("{id}/comment")]
        public async Task<IActionResult> CreateComment(Guid id, [FromBody] Post model)
        {
            var post = await _postService.Get(id);

            model.UserId = UserId;

            if (post != null)
            {
                post.Comments.Add(model);

                await _postService.Update(id, post);
            }

            return Map(post, false);
        }
    }
}
