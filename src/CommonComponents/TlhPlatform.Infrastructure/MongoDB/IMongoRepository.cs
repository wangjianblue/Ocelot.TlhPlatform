using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using TlhPlatform.Infrastructure.MongoDB.Base;

namespace TlhPlatform.Infrastructure.MongoDB
{
    public interface IMongoRepository
    {
        bool Add<T>(T entity);
        Task AddAsync<T>(T entity);
        bool AddIfNotExist<T>(Expression<Func<T, bool>> predicate, T entity); 
        Task<bool> AddIfNotExistAsync<T>(Expression<Func<T, bool>> predicate, T entity);
        void BatchAdd<T>(IEnumerable<T> entities);
        Task BatchAddAsync<T>(List<T> entity);
        long Count<T>(Expression<Func<T, bool>> predicate);
        long Delete<T>(Expression<Func<T, bool>> predicate);
        Task<long> DeleteAsync<T>(Expression<Func<T, bool>> predicate);
        void DropCollection(string collection);
        bool Exists<T>(Expression<Func<T, bool>> predicate);
        TResult Get<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        TResult Get<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort);
        T Get<T>(Expression<Func<T, bool>> predicate);
        T Get<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> selector, Func<Sort<T>, Sort<T>> sort);
        T Get<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort);
        T GetAndUpdate<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression);
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null);
        IMongoCollection<T> GetCollection<T>();
        List<string> ListCollections();
        PageList<TResult> PageList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize);
        PageList<TResult> PageList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, int pageIndex, int pageSize) where TResult : class;
        PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize);
        PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize);
        Task<PageList<T>> PageListAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int pageIndex = 1, int pageSize = 20, Expression<Func<T, object>> orderby = null, bool desc = false);
        long Set<T>(Expression<Func<T, bool>> predicate, T t);
        Task<long> SetAsync<T>(Expression<Func<T, bool>> predicate, T t);
        List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort);
        List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int? top);
        List<T> ToList<T>(Expression<Func<T, bool>> predicate);
        List<T> ToList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int? top);
        List<T> ToList<T>(Expression<Func<T, bool>> predicate, int top);
        Task<List<T>> ToListAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int? limit = null);
        long Update<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda);
        long Update<T>(Expression<Func<T, bool>> predicate, T entity);
        Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda);
        Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, T entity);
    }
}