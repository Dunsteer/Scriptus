using Commons.Models.User;
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
    public class UserDB: BaseMongoDB<User>
    {
        public UserDB(IMongoDatabaseSettings settings) : base(settings)
        {

        }

        public override async Task<IQueryable<User>> ReadMany(object o, bool populate = false)
        {
            var search = o as UserSearchModel;
            if (search == null) return await base.ReadMany(search, populate);

            var builder = Builders<User>.Filter;
            var sorter = Builders<User>.Sort;

            FilterDefinition<User> filter = builder.Empty;
            SortDefinition<User> sort = sorter.Descending("LogTime");

            if (!String.IsNullOrEmpty(search.Id))
            {
                filter = filter & builder.Eq("Id", search.Id);
            }

            if (!String.IsNullOrEmpty(search.Username))
            {
                filter = filter & builder.Where(u => u.Username.ToLower().Contains(search.Username.ToLower()));
            }

            if (!String.IsNullOrEmpty(search.Password))
            {
                filter = filter & builder.Where(u => u.Password.Contains(search.Password));
            }

            if (!String.IsNullOrEmpty(search.Email))
            {
                filter = filter & builder.Where(u => u.Email.Contains(search.Email) || u.AditionalEmails.Contains(search.Email));
            }

            //if (!String.IsNullOrEmpty(search.SortMember))
            //{
            //    search.SortMember = char.ToUpper(search.SortMember.First()) + search.SortMember.Substring(1);

            //    if (search.SortOrder)
            //        sort = sorter.Ascending(search.SortMember);
            //    else
            //        sort = sorter.Descending(search.SortMember);

            //    if (search.SortMember != "LogTime")
            //    {
            //        sort = sort.Descending("LogTime");
            //    }
            //}

            var result = _collection.Find(filter);

            if (search.Page.HasValue && search.PageSize.HasValue && search.PageSize.HasValue)
            {
                var skip = search.Page.Value * search.PageSize.Value;
                var take = search.PageSize.Value;

                return result.Sort(sort).Skip(skip).Limit(take).ToList().AsQueryable();
            }

            return result.Sort(sort).ToList().AsQueryable();
        }

        public async Task<User> CanLogin(string username, string password_hash)
        {
            return (await this.ReadMany(new UserSearchModel { Username = username, Password = password_hash })).FirstOrDefault();
        }
    }
}
