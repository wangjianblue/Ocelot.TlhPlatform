using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using TlhPlatform.Infrastructure.MongoDB.Base;
using TlhPlatform.Infrastructure.MongoDB.Extension;

namespace TlhPlatform.Infrastructure.MongoDB
{
    #region MongoDb操作封装

    /// <summary>
    /// MongoDb操作封装
    /// </summary>
    public class MongoRepository : IMongoRepository
    {
        #region 初始化

        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _db;
        private readonly MongoOption _option;

        public MongoRepository(MongoOption option)
        {
            _option = option;
            _mongoClient = new MongoClient(_option.ConnectionString);
            _db = _mongoClient.GetDatabase(_option.DatabaseName);
        }

        static MongoRepository()
        {
            ConventionRegistry.Register("IgnoreExtraElements",
                new ConventionPack { new IgnoreExtraElementsConvention(true) }, type => true);
        }

        #endregion

        #region 库、集合

        private static string InferCollectionNameFrom<T>()
        {
            var type = typeof(T);
            return type.Name;
        }
        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = InferCollectionNameFrom<T>();
            return _db.GetCollection<T>(collectionName);

        }
        //public List<string> ListDatabases()
        //{
        //    var dbList = new List<string>();
        //    using (var cursor = _mongoClient.ListDatabases())
        //    {
        //        cursor.ForEachAsync(d => dbList.Add(d.ToString())).ConfigureAwait(false);
        //    }
        //    return dbList;
        //}
        public List<string> ListCollections()
        {
            var collList = new List<string>();
            using (var cursor = _db.ListCollections())
            {
                cursor.ForEachAsync(d => collList.Add(d.ToString())).ConfigureAwait(false);
            }
            return collList;
        }
        public void DropCollection(string collection)
        {
            _db.DropCollection(collection);
        }

        #endregion

        #region 增

        #region 增（同步）

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public bool Add<T>(T entity)
        {
            var coll = GetCollection<T>();
            coll.InsertOne(entity);
            return true;
        }

        #endregion

        #region 增（异步）

        /// <summary>
        /// 增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        /// <returns></returns>
        public Task AddAsync<T>(T entity)
        {
            var coll = GetCollection<T>();
            return coll.InsertOneAsync(entity);
        }

        #endregion

        #endregion

        #region 无则新增
        /// <summary>
        /// 无则新增
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddIfNotExist<T>(Expression<Func<T, bool>> predicate, T entity)
        {
            var coll = GetCollection<T>();
            if (coll.CountDocuments(predicate) > 0)
                return false;
            coll.InsertOne(entity);
            return true;
        }

        /// <summary>
        /// 无则新增
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddIfNotExistAsync<T>(Expression<Func<T, bool>> predicate, T entity)
        {
            var coll = GetCollection<T>();
            var cout = await coll.CountDocumentsAsync(predicate);
            if (cout > 0)
                return false;
            coll.InsertOne(entity);
            return true;
        }



        #endregion

        #region 批量增

        #region 批量增（异步）

        /// <summary>
        /// 批量增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public Task BatchAddAsync<T>(List<T> entity)
        {
            var coll = GetCollection<T>();

            return coll.InsertManyAsync(entity);
        }

        #endregion

        #region 批量增（同步）

        /// <summary>
        /// 批量增（同步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void BatchAdd<T>(IEnumerable<T> entities)
        {
            var coll = GetCollection<T>();

            coll.InsertMany(entities);
        }

        #endregion

        #endregion

        #region 新增更新
        #region 同步
        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public long Set<T>(Expression<Func<T, bool>> predicate, T t)
        {
            var coll = GetCollection<T>();
            var reuslt = coll.ReplaceOne(predicate, t, new UpdateOptions { IsUpsert = true });
            return reuslt.ModifiedCount;
        }

        #endregion

        #region 异步
        /// <summary>
        /// 新增更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<long> SetAsync<T>(Expression<Func<T, bool>> predicate, T t)
        {
            var coll = GetCollection<T>();
            var reuslt = await coll.ReplaceOneAsync(predicate, t, new UpdateOptions { IsUpsert = true });
            return reuslt.ModifiedCount;
        }

        #endregion
        #endregion

        #region 删

        #region 删（同步）


        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public long Delete<T>(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection<T>();
            var result = coll.DeleteMany(predicate).DeletedCount;
            return result;
        }

        #endregion

        #region 删（异步）

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection<T>();
            var result = await coll.DeleteManyAsync(predicate);
            return result.DeletedCount;
        }

        #endregion

        #endregion

        #region 改

        #region 改（同步）

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="entity">字段</param>
        /// <returns></returns>
        public long Update<T>(Expression<Func<T, bool>> predicate, T entity)
        {
            var coll = GetCollection<T>();
            var updateDefinitionList = entity.GetUpdateDefinition();
            var result = coll.UpdateOne<T>(predicate, updateDefinitionList);
            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public long Update<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda)
        {
            var coll = GetCollection<T>();

            var updateDefinitionList = MongoExpression<T>.GetUpdateDefinition(lambda);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = coll.UpdateMany<T>(predicate, updateDefinitionBuilder);

            return result.ModifiedCount;
        }

        #endregion

        #region 改（异步）

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, T entity)
        {
            var coll = GetCollection<T>();
            var updateDefinitionList = entity.GetUpdateDefinition();
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);
            var result = await coll.UpdateOneAsync<T>(predicate, updateDefinitionBuilder);
            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="lambda">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda)
        {
            var coll = GetCollection<T>();
            var updateDefinitionList = MongoExpression<T>.GetUpdateDefinition(lambda);
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);
            var result = await coll.UpdateManyAsync<T>(predicate, updateDefinitionBuilder);
            return result.ModifiedCount;
        }

        #endregion

        #endregion

        #region 查

        #region 查（同步）

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate, null);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort)
        {
            return Get<T>(predicate, a => a, sort);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector"></param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> selector, Func<Sort<T>, Sort<T>> sort)
        {
            return Get<T, T>(predicate, selector, sort);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="selector">查询字段</param>
        /// <returns></returns>
        public TResult Get<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Get(predicate, selector, null);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public TResult Get<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort)
        {
            var coll = GetCollection<T>();
            var find = coll.Find(predicate);
            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());
            return find.Project(selector).FirstOrDefault();
        }

        #endregion

        #region 查（异步）
        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>       
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null)
        {
            var coll = GetCollection<T>();
            var find = coll.Find(predicate);
            if (projector != null)
                find = find.Project(projector);
            return find.FirstOrDefaultAsync();
        }

        //public IQueryable<T> Get<T>(int id, string name, string address, DateTime createTime)
        //{
        //    Expression<Func<T, bool>> getFunc = x =>
        //       (id == 0) &&
        //       (string.IsNullOrWhiteSpace(name)) &&
        //       (string.IsNullOrWhiteSpace(address)) &&
        //       (createTime == null);

        //    var coll = GetCollection<T>();
        //    var find = coll.Find(getFunc);
          
        //}

        #endregion

        #endregion

        #region 列表

        #region 列表（同步）

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="sort">排序</param>
        /// <param name="top">取X</param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int? top)
        {
            var coll = GetCollection<T>();
            var find = coll.Find(predicate);
            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());
            if (top != null)
                find = find.Limit(top);
            return find.Project(selector).ToList();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort)
        {
            return ToList(predicate, selector, sort, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public List<TResult> ToList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return ToList(predicate, selector, null);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> ToList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int? top)
        {
            return ToList(predicate, a => a, sort, top);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<T> ToList<T>(Expression<Func<T, bool>> predicate, int top)
        {
            return ToList(predicate, null, top);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> ToList<T>(Expression<Func<T, bool>> predicate)
        {
            return ToList(predicate, null, null);
        }

        #endregion

        #region 列表（异步）
        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Task<List<T>> ToListAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int? limit = null)
        {
            var coll = GetCollection<T>();
            var find = coll.Find(predicate);
            if (projector != null)
                find = find.Project(projector);
            if (limit != null)
                find = find.Limit(limit);
            return find.ToListAsync();
        }

        #endregion

        #endregion

        #region 分页

        #region 分页（同步）

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>

        /// <param name="predicate">过滤条件</param>
        /// <param name="sort">排序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>

        /// <param name="selector">查询字段</param>
        /// <returns></returns>
        [Obsolete]
        public PageList<TResult> PageList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize)
        {
            var coll = GetCollection<T>();
           // coll.Indexes.CreateOne(new CreateIndexModel<T>(new BsonDocument("_id", 1)));
            var count = (int)coll.Count<T>(predicate);
            var find = coll.Find(predicate);
            if (sort != null)
                find = find.Sort(sort.GetSortDefinition());
            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);
            var items = find.Project(selector).ToList();
            return new PageList<TResult>(pageIndex, pageSize, count, items);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="selector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        /// <returns></returns>
        public PageList<TResult> PageList<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, int pageIndex, int pageSize) where TResult : class
        {
            return PageList(predicate, selector, null, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        public PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize)
        {
            return PageList(predicate, a => a, sort, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页长</param>
        public PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        {
            return PageList(predicate, null, pageIndex, pageSize);
        }

        #endregion

        #region 分页（异步）
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int pageIndex = 1, int pageSize = 20,
            Expression<Func<T, object>> orderby = null, bool desc = false)
        {
            var coll = GetCollection<T>();
            var count = (int)await coll.CountAsync<T>(predicate);
            var find = coll.Find(predicate);
            if (projector != null)
                find = find.Project(projector);
            if (orderby != null)
                find = desc ? find.SortByDescending(@orderby) : find.SortBy(@orderby);
            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);
            var items = await find.ToListAsync();
            return new PageList<T>(pageIndex, pageSize, count, items);
        }

        #endregion

        #endregion

        #region 是否存在
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public bool Exists<T>(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection<T>();
            var result = coll.Count(predicate);
            return result > 0;
        }
        #endregion

        #region 条数
        /// <summary>
        /// 按条件查询条数
        /// </summary>       
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public long Count<T>(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection<T>();
            return coll.Count(predicate);
        }
        #endregion

        #region 原子操作

        /// <summary>
        /// 查询出更新结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public T GetAndUpdate<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateExpression)
        {
            var col = GetCollection<T>();
            var updateDefinitionList = MongoExpression<T>.GetUpdateDefinition(updateExpression);
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);
            return col.FindOneAndUpdate(predicate, updateDefinitionBuilder, new FindOneAndUpdateOptions<T, T>
            {
                ReturnDocument = ReturnDocument.After
            });
        }
        #endregion
    }

    #endregion
}
