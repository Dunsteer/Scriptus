using BaseLogic.Abstractions;
using Commons.Core;
using DbServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLogic.Services
{
    public class PostService : BaseService<Post>
    {
        public UserService _userService;
        public PostService(IDataProvider<Post> db,BaseService<User> userService, MapperService mapper) : base(db, mapper)
        {
            _userService = (UserService)userService;
        }

        public override async ValueTask<Post> Get(object id)
        {
            var post = await base.Get(id);

            var dic = new Dictionary<Guid, User>();

            if (dic.ContainsKey(post.UserId))
            {
                post.User = dic[post.UserId];
            }
            else
            {
                dic[post.UserId] = await _userService.Get(post.UserId);
                post.User = dic[post.UserId];
            }

            foreach (var comment in post.Comments)
            {
                if (dic.ContainsKey(comment.UserId))
                {
                    comment.User = dic[comment.UserId];
                }
                else
                {
                    dic[comment.UserId] = await _userService.Get(comment.UserId);
                    comment.User = dic[comment.UserId];
                }
            }

            return post;
        }

        public override async Task<IQueryable<Post>> GetAll(object o, bool withInclude = true)
        {
            var res = (await base.GetAll(o, withInclude)).ToList();
            if (withInclude)
            {
                var dic = new Dictionary<Guid, User>();

                foreach(var post in res)
                {
                    if (dic.ContainsKey(post.UserId))
                    {
                        post.User = dic[post.UserId];
                    }
                    else
                    {
                        dic[post.UserId] = await _userService.Get(post.UserId);
                        post.User = dic[post.UserId];
                    }
                }
            }
            return res.AsQueryable();
        }
    }
}
