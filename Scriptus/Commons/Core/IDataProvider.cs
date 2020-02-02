using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Core
{
    public interface IDataProvider<DB> where DB : class
    {
        Task<DB> CreateOne(DB model);
        Task<List<DB>> CreateMany(List<DB> models);
        ValueTask<DB> ReadOne(object id);
        Task<IQueryable<DB>> ReadMany(object search, bool populate = false);
        Task<DB> UpdateOne(object id, DB model);
        Task<List<DB>> UpdateMany(List<object> ids, List<DB> models);
        Task<DB> PatchOne(object id, JsonPatchDocument<DB> patch);
        Task<List<DB>> PatchMany(List<object> ids, List<JsonPatchDocument<DB>> patches);
        Task<DB> DeleteOne(object id);
        Task<List<DB>> DeleteMany(List<object> ids);
        Task<List<DB>> CreateOrUpdateBulk(List<DB> list);
        Task SaveChanges();
        Task<IQueryable<DB>> Search(IDictionary<string, string> search);
        Task Refresh(DB entity);
    }
}
