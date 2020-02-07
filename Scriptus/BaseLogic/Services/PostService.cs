using BaseLogic.Abstractions;
using Commons.Core;
using DbServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLogic.Services
{
    public class PostService : BaseService<Post>
    {
        public PostService(IDataProvider<Post> db, MapperService mapper) : base(db, mapper)
        {
        }
    }
}
