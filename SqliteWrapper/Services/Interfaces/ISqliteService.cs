using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Entities;

namespace Famoser.SqliteWrapper.Services.Interfaces
{
    public interface ISqliteService
    {
        /// <summary>
        /// Retrieve the item with the specified id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetById<T>(int id) where T : EntityBase, new();
        /// <summary>
        /// Retrieve all items with the specified ids
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<T>> GetAllById<T>(IEnumerable<int> ids) where T : EntityBase, new();
        /// <summary>
        /// Retrieve all content of the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetAll<T>() where T : EntityBase, new();
        /// <summary>
        /// Delete the item with the specified id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById<T>(int id) where T : EntityBase, new();
        /// <summary>
        /// Delete all items with the specified ids
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAllById<T>(IEnumerable<int> ids) where T : EntityBase, new();
        /// <summary>
        /// Delete all items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> DeleteAll<T>() where T : EntityBase, new();
        /// <summary>
        /// Add item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> Add<T>(T entity) where T : EntityBase, new();
        /// <summary>
        /// Add all items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> AddAll<T>(IEnumerable<T> entity) where T : EntityBase, new();
        /// <summary>
        /// Update item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> Update<T>(T entity) where T : EntityBase, new();
        /// <summary>
        /// Update all items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> UpdateAll<T>(IEnumerable<T> entity) where T : EntityBase, new();
        /// <summary>
        /// Retrieve item with the highest id from the specified table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<int> GetHighestId<T>() where T : EntityBase, new();
        /// <summary>
        /// Get a list of items with match the specified conditions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="descending"></param>
        /// <param name="limit"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        Task<List<T>> GetByCondition<T>(Expression<Func<T, bool>> func, Expression<Func<T, object>> orderByProperty, bool descending, int limit, int skip) where T : EntityBase, new();
        /// <summary>
        /// Count all items with match the specified condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<int> CountByCondition<T>(Expression<Func<T, bool>> func) where T : EntityBase, new();

        /// <summary>
        /// execute a raw query and retrieve an integer.
        /// The integer is -1 if the query was unsuccessfull
        /// </summary>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync<T>(string query, params object[] args) where T : EntityBase, new();

        /// <summary>
        /// execute a raw query and retrieve a scalar (e. g. "SELECT COUNT(*) FROM users WHERE prename=Margareth)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string query, params object[] args) where T : EntityBase, new();
    }
}
