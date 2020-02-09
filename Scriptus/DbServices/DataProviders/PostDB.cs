using Commons.Models.Post;
using DbServices.Abstractions;
using DbServices.Models;
using MongoDB.Driver;
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
        public override async Task<IQueryable<Post>> ReadMany(object o, bool populate)
        {
            var search = o as PostSearchModel;
            if (search == null) return await base.ReadMany(search, populate);

            var builder = Builders<Post>.Filter;
            var sorter = Builders<Post>.Sort;

            FilterDefinition<Post> filter = builder.Empty;
            SortDefinition<Post> sort = sorter.Descending("LogTime");

            if (search.Tags != null && search.Tags.Length > 0)
            {
                foreach (var tag in search.Tags)
                {
                    if (!string.IsNullOrWhiteSpace(tag))
                    {
                        filter = filter & builder.Where(p => p.Tags.Any(x => x.ToLower().Contains(tag.ToLower())));
                    }
                }
            }

            var result = _collection.Find(filter);

            if (search.Page.HasValue && search.PageSize.HasValue && search.PageSize.HasValue)
            {
                var skip = search.Page.Value * search.PageSize.Value;
                var take = search.PageSize.Value;

                return result.Sort(sort).Skip(skip).Limit(take).ToList().AsQueryable();
            }

            return result.Sort(sort).ToList().AsQueryable();
        }
    }
}
