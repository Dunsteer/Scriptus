using BaseLogic.Services;
using Commons.Core;
using DbServices.DataProviders;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLogic.Abstractions
{
    public abstract class BaseService<DB> where DB : class
    {
        protected IDataProvider<DB> _database;
        protected MapperService _mapper;

        public BaseService(IDataProvider<DB> database, MapperService mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public virtual Task<IQueryable<DB>> GetAll(object o, bool withInclude = true)
        {
            return _database.ReadMany(o, withInclude);
        }

        public virtual ValueTask<DB> Get(object id)
        {
            return _database.ReadOne(id);
        }

        public virtual Task<DB> Create(DB model)
        {
            return _database.CreateOne(model);
        }

        public virtual Task<List<DB>> CreateMany(List<DB> models)
        {
            return _database.CreateMany(models);
        }

        public virtual Task<DB> Update(object id, DB model)
        {
            return _database.UpdateOne(id, model);
        }

        public virtual Task<DB> Patch(object id, JsonPatchDocument<DB> patch)
        {
            return _database.PatchOne(id, patch);
        }

        public virtual Task<DB> Delete(object id)
        {
            return _database.DeleteOne(id);
        }

        public virtual Task<List<DB>> CreateOrUpdateBulk(List<DB> list)
        {
            return _database.CreateOrUpdateBulk(list);
        }

        public virtual Task<IQueryable<DB>> Search(IDictionary<string, string> search)
        {
            return _database.Search(search);
        }

        public virtual Task SaveChanges()
        {
            return _database.SaveChanges();
        }

        public virtual Task Refresh(DB entity)
        {
            return _database.Refresh(entity);
        }
    }
}
