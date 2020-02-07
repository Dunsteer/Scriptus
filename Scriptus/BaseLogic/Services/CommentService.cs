using BaseLogic.Abstractions;
using Commons.Core;
using DbServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLogic.Services
{
    public class CommentService : BaseService<Comment>
    {
        public CommentService(IDataProvider<Comment> db, MapperService mapper) : base(db, mapper)
        {
        }
    }
}
