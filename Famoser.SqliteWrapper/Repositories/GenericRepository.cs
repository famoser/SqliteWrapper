using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Famoser.FrameworkEssentials.Logging.Interfaces;
using Famoser.SqliteWrapper.Entities;
using Famoser.SqliteWrapper.Helpers;
using Famoser.SqliteWrapper.Models.Interfaces;
using Famoser.SqliteWrapper.Services.Interfaces;

namespace Famoser.SqliteWrapper.Repositories
{
    public class GenericRepository<TBusiness, TEntity>
             where TBusiness : class, ISqliteModel, new()
             where TEntity : BaseEntity, new()
    {
        private readonly ISqliteService _dataService;
        private readonly IExceptionLogger _exceptionLogger;

        public GenericRepository(ISqliteService dataService, IExceptionLogger exceptionLogger = null)
        {
            _dataService = dataService;
            _exceptionLogger = exceptionLogger;
        }

        private void LogOrThrow(Exception ex)
        {
            if (_exceptionLogger != null)
                _exceptionLogger.LogException(ex);
            else
                throw ex;
        }

        public async Task<TBusiness> GetById(int id)
        {
            try
            {
                var entity = await _dataService.GetById<TEntity>(id);

                if (entity != null)
                {
                    var business = new TBusiness();
                    business = MappingHelper.ConvertToModel(entity, business);

                    return business;
                }
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return null;
        }

        public async Task<bool> Delete(TBusiness business)
        {
            try
            {
                var entity = MappingHelper.ConvertToEntity(business, new TEntity());
                if (await Delete(entity.Id))
                {
                    business.SetId(0);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                return await _dataService.DeleteById<TEntity>(id);
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<List<TBusiness>> GetByCondition(Expression<Func<TEntity, bool>> func, Expression<Func<TEntity, object>> orderByProperty = null, bool descending = false, int limit = 0, int skip = 0)
        {
            try
            {
                var entityList = await _dataService.GetByCondition(func, orderByProperty, descending, limit, skip);

                if (entityList.Any())
                {
                    return (from entity in entityList let business = new TBusiness() select MappingHelper.ConvertToModel(entity, business)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return new List<TBusiness>();
        }

        public async Task<List<TBusiness>> GetAll()
        {
            try
            {
                var entityList = await _dataService.GetAll<TEntity>();

                if (entityList.Any())
                {
                    return (from entity in entityList let business = new TBusiness() select MappingHelper.ConvertToModel(entity, business)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return new List<TBusiness>();
        }

        public async Task<int> CountByCondition(Expression<Func<TEntity, bool>> func)
        {
            try
            {
                return await _dataService.CountByCondition(func);
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return -1;
        }

        public async Task<bool> Add(TBusiness business)
        {
            try
            {
                var entity = MappingHelper.ConvertToEntity(business, new TEntity());
                int id = await _dataService.Add(entity);
                if (id == 1)
                {
                    business.SetId(entity.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<bool> AddAll(List<TBusiness> business)
        {
            try
            {
                var list = MappingHelper.ConvertAllToEntity<TEntity, TBusiness>(business);
                var res = await _dataService.AddAll(list);
                if (res == business.Count)
                {
                    for (int index = 0; index < business.Count; index++)
                    {
                        business[index].SetId(list[index].Id);
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<bool> Update(TBusiness business)
        {
            try
            {
                var entity = MappingHelper.ConvertToEntity(business, new TEntity());
                return await _dataService.Update(entity) == 1;
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<bool> UpdateAll(List<TBusiness> business)
        {
            try
            {
                var list = MappingHelper.ConvertAllToEntity<TEntity, TBusiness>(business);
                return await _dataService.UpdateAll(list) == list.Count;
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public Task<bool> Save(TBusiness business)
        {
            if (business.GetId() != 0)
                return Update(business);
            return Add(business);
        }

        public async Task<bool> SaveAll(List<TBusiness> business)
        {
            try
            {
                var news = new List<TBusiness>();
                var updates = new List<TBusiness>();
                foreach (var business1 in business)
                {
                    if (business1.GetId() != 0)
                        updates.Add(business1);
                    else
                        news.Add(business1);
                }

                return await AddAll(news) && await UpdateAll(updates);
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<bool> DeleteAll(List<TBusiness> business)
        {
            try
            {
                var list = MappingHelper.ConvertAllToEntity<TEntity, TBusiness>(business);
                var ids = list.Select(l => l.Id).ToList();
                if (await DeleteAll(ids))
                {
                    foreach (var business1 in business)
                    {
                        business1.SetId(0);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }

        public async Task<bool> DeleteAll(List<int> ids)
        {
            try
            {
                return await _dataService.DeleteAllById<TEntity>(ids);
            }
            catch (Exception ex)
            {
                LogOrThrow(ex);
            }
            return false;
        }
    }
}
