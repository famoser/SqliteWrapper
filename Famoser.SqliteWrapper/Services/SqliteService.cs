using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Entities;
using Famoser.SqliteWrapper.Services.Interfaces;
using Nito.AsyncEx;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace Famoser.SqliteWrapper.Services
{
    public class SqliteService : ISqliteService
    {
        private readonly ISQLitePlatform _sqLitePlatform;
        private readonly ISqliteServiceSettingsProvider _sqliteServiceSettingsProvider;
        private static readonly AsyncLock DatabaseLock = new AsyncLock();

        public SqliteService(ISQLitePlatform sqlitePlatform, ISqliteServiceSettingsProvider sqliteServiceSettingsProvider)
        {
            _sqLitePlatform = sqlitePlatform;
            _sqliteServiceSettingsProvider = sqliteServiceSettingsProvider;
        }

        private SQLiteAsyncConnection _myAsyncConnection;
        private SQLiteConnectionWithLock _myConnection;
        private async Task<SQLiteAsyncConnection> GetConnection<T>() where T : BaseEntity
        {
            if (_myAsyncConnection == null)
            {
                string databaseFile = await _sqliteServiceSettingsProvider.GetFullPathOfDatabase();
                _myConnection = new SQLiteConnectionWithLock(_sqLitePlatform, new SQLiteConnectionString(databaseFile, false));
                _myAsyncConnection = new SQLiteAsyncConnection(() => _myConnection);
                await _myAsyncConnection.ExecuteAsync("PRAGMA synchronous = OFF");
                await _sqliteServiceSettingsProvider.DoMigration(_myAsyncConnection);
            }
            await CreateTableIfNeeded<T>();
            return _myAsyncConnection;
        }

        private readonly ConcurrentBag<Type> _existingTables = new ConcurrentBag<Type>();
        private async Task CreateTableIfNeeded<T>() where T : BaseEntity
        {
            if (!_existingTables.Contains(typeof(T)))
            {
                await _myAsyncConnection.CreateTableAsync<T>();
                _existingTables.Add(typeof(T));
            }
        }

        public async Task<T> GetById<T>(int id) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                var res = await (await GetConnection<T>()).GetAsync<T>(id);
                return res;
            }
        }

        public async Task<List<T>> GetAllById<T>(IEnumerable<int> ids) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                var res = await (await GetConnection<T>()).Table<T>().Where(a => ids.Any(d => d == a.Id)).ToListAsync();
                return res;
            }
        }

        public async Task<List<T>> GetAll<T>() where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                var res = await (await GetConnection<T>()).Table<T>().ToListAsync();
                return res;
            }
        }

        public async Task<bool> DeleteAllById<T>(IEnumerable<int> ids) where T : BaseEntity, new()
        {
            var args = string.Join(",", ids);
            using (await DatabaseLock.LockAsync())
            {
                var query = new TableQuery<T>(_sqLitePlatform, _myConnection);
                await (await GetConnection<T>()).ExecuteAsync("DELETE FROM " + query.Table.TableName + " WHERE id IN (" + args + ");");
            }
            return true;
        }

        public async Task<bool> DeleteAll<T>() where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                await (await GetConnection<T>()).DeleteAllAsync<T>();
            }
            return true;
        }

        public async Task<int> Add<T>(T obj) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                return await (await GetConnection<T>()).InsertAsync(obj);
            }
        }

        public async Task<int> AddAll<T>(IEnumerable<T> obj) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                return await (await GetConnection<T>()).InsertAllAsync(obj);
            }
        }

        public async Task<int> Update<T>(T obj) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                return await (await GetConnection<T>()).UpdateAsync(obj);
            }
        }

        public async Task<int> UpdateAll<T>(IEnumerable<T> obj) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                return await (await GetConnection<T>()).UpdateAllAsync(obj);
            }
        }

        public async Task<int> GetHighestId<T>() where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                var s = await (await GetConnection<T>()).Table<T>().OrderByDescending(c => c.Id).FirstAsync();
                return s.Id;
            }
        }

        public async Task<bool> DeleteById<T>(int id) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                await (await GetConnection<T>()).DeleteAsync<T>(id);
            }
            return true;
        }

        public async Task<List<T>> GetByCondition<T>(Expression<Func<T, bool>> func, Expression<Func<T, object>> orderByProperty = null, bool descending = false, int limit = 0, int skip = 0) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                if (orderByProperty != null)
                {
                    if (descending)
                    {
                        if (limit > 0)
                            return await (await GetConnection<T>()).Table<T>()
                                        .Where(func)
                                        .OrderByDescending(orderByProperty)
                                        .Skip(skip)
                                        .Take(limit)
                                        .ToListAsync();
                        return await (await GetConnection<T>()).Table<T>()
                                        .Where(func)
                                        .OrderByDescending(orderByProperty)
                                        .Skip(skip)
                                        .ToListAsync();
                    }
                    if (limit > 0)
                        return await (await GetConnection<T>()).Table<T>()
                                    .Where(func)
                                    .OrderBy(orderByProperty)
                                    .Skip(skip)
                                    .Take(limit)
                                    .ToListAsync();
                    return await (await GetConnection<T>()).Table<T>()
                                .Where(func)
                                .OrderBy(orderByProperty)
                                .Skip(skip)
                                .ToListAsync();
                }
                if (limit > 0)
                    return await (await GetConnection<T>()).Table<T>().Where(func).Take(limit).Skip(skip).ToListAsync();
                return await (await GetConnection<T>()).Table<T>().Where(func).Skip(skip).ToListAsync();
            }
        }

        public async Task<int> CountByCondition<T>(Expression<Func<T, bool>> func) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                var res = await (await GetConnection<T>()).Table<T>().Where(func).CountAsync();
                return res;
            }
        }

        public async Task<int> ExecuteAsync<T>(string query, params object[] args) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                return await (await GetConnection<T>()).ExecuteAsync(query, args);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, params object[] args) where T : BaseEntity, new()
        {
            using (await DatabaseLock.LockAsync())
            {
                return await (await GetConnection<T>()).ExecuteScalarAsync<T>(query, args);
            }
        }
    }
}
