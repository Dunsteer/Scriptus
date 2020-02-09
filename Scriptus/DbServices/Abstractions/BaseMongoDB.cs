using Commons.Core;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Abstractions
{
    public abstract class BaseMongoDB<DB> : IDataProvider<DB> where DB : class, new()
    {
        protected virtual IMongoCollection<DB> _collection { get; set; }

        protected virtual string PropertyIdName => $"Id";

        public object DbContext => throw new NotImplementedException();

        public BaseMongoDB(IMongoDatabaseSettings settings, string collectioName = null)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            if (collectioName == null) collectioName = typeof(DB).Name;

            _collection = database.GetCollection<DB>(collectioName);
        }

        public virtual async ValueTask<DB> ReadOne(object id)
        {
            var filter = Builders<DB>.Filter.Eq(PropertyIdName, id);

            var result = await _collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public virtual async Task<IQueryable<DB>> ReadMany(object search, bool populate)
        {
            return _collection.Find(db => true).ToList().AsQueryable();
        }

        public async Task<List<DB>> CreateMany(List<DB> models)
        {
            await _collection.InsertManyAsync(models);
            return models;
        }

        public async Task<DB> CreateOne(DB model)
        {
            await _collection.InsertOneAsync(model,new InsertOneOptions { BypassDocumentValidation = true});
            return model;
        }

        public Task<List<DB>> CreateOrUpdateBulk(List<DB> list)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DB>> DeleteMany(List<object> ids)
        {
            var filter = Builders<DB>.Filter.AnyEq(PropertyIdName, ids);

            var result = await _collection.DeleteManyAsync(filter);
            return new List<DB>();
        }

        public async Task<DB> DeleteOne(object id)
        {
            var filter = Builders<DB>.Filter.Eq(PropertyIdName, id);

            var result = await _collection.DeleteOneAsync(filter);
            return new DB();
        }

        public async Task<DB> PatchOne(object id, JsonPatchDocument<DB> patch)
        {
            var data = await ReadOne(id);
            if (data != null)
            {
                patch.ApplyTo(data);

                var filter = Builders<DB>.Filter.Eq(PropertyIdName, id);
                var result = await _collection.ReplaceOneAsync(filter, data);
                if (result == null) return null;

                return data;
            }

            return null;
        }

        public Task<List<DB>> PatchMany(List<object> ids, List<JsonPatchDocument<DB>> patches)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DB> Query(bool withInclude = false)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<DB>> Search(IDictionary<string, string> search)
        {
            throw new NotImplementedException();
        }

        public async Task<DB> UpdateOne(object id, DB model)
        {
            var filter = Builders<DB>.Filter.Eq(PropertyIdName, id);

            var result = await _collection.ReplaceOneAsync(filter, model);
            if (result.IsAcknowledged) return model;
            else return null;
        }

        public Task<List<DB>> UpdateMany(List<object> ids, List<DB> models)
        {
            throw new NotImplementedException();
        }

        public Task Refresh(DB entity)
        {
            throw new NotImplementedException();
        }
    }
}
