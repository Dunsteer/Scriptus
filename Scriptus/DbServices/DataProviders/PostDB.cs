using DbServices.Abstractions;
using DbServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbServices.DataProviders
{
    public class PostDB : BaseMongoDB<Post>
    {
        public PostDB(IMongoDatabaseSettings settings) : base(settings)
        {

        }


    }
}
