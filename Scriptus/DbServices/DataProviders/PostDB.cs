using DbServices.Abstractions;
using DbServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.DataProviders
{
    public class PostDB : BaseMongoDB<Post>
    {
        public PostDB(IMongoDatabaseSettings settings) : base(settings)
        {

        }
        public override Task<IQueryable<Post>> ReadMany(object search, bool populate)
        {
            return base.ReadMany(search, populate);
        }

    }
}
