using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Infrastructure.AutoMapper
{
    public static class MappingExtensions
    {
        #region Utilities
        /// <summary>
        /// Map 扩展方法
        /// </summary>
        /// <typeparam name="TDestination">要映射的目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标类型的对象</returns>
        private static TDestination Map<TDestination>(this object source)
        {
            //use AutoMapper for mapping objects
            return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// MapTo 扩展方法
        /// </summary>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <typeparam name="TDestination">要映射的目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">待更新的目标对象</param>
        /// <returns>更新后的目标类型对象</returns>
        private static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            //use AutoMapper for mapping objects
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #endregion 


        #region ToModel   

        #region View
        /// <summary>
        /// 将 BaseEntity实体转换成 ViewModel
        /// </summary>
        /// <typeparam name="TModel">要映射的目标类型</typeparam>
        /// <param name="baseEntity">源对象</param>
        /// <returns>更新后的目标类型对象</returns> 

        public static TModel ToModel<TModel>(this IBaseEntity entity)
            where TModel : IBaseDtoModel
        {
            if (entity == null)
                return default(TModel);

            return entity.Map<TModel>();
        }

        /// <summary>
        /// 将 BaseEntity实体转换成 ViewModel
        /// </summary>
        /// <typeparam name="TEntity">源对象</typeparam>
        /// <typeparam name="TModel">要映射的目标类型</typeparam>
        /// <param name="entity">源对象</param>
        /// <param name="model">要映射的目标类型</param>
        /// <returns>更新后的目标类型对象</returns>
        public static TModel ToModel<TEntity, TModel>(this TEntity entity, TModel model)
            where TModel : IBaseDtoModel
            where TEntity : IBaseEntity
        {
            if (entity == null)
                return default(TModel);
            if (model == null)
                return default(TModel);

            return entity.MapTo(model);
        }


        #endregion

        #region IEnumerable
        /// <summary>
        /// 将 IEnumerable<BaseEntity>实体转换成 IEnumerable<ViewModel>
        /// </summary>
        /// <typeparam name="TModel">要映射的目标类型</typeparam>
        /// <param name="baseEntity">源对象</param>
        /// <returns>更新后的目标类型对象</returns>
        public static IEnumerable<TModel> ToModels<TModel>(this IEnumerable<IBaseEntity> baseEntity)
            where TModel : IBaseDtoModel
        {
            if (baseEntity == null)
                return default(IEnumerable<TModel>);
            return baseEntity.Map<IEnumerable<TModel>>();
        }


        /// <summary>
        /// 将 IEnumerable<BaseEntity>实体转换成 IEnumerable<ViewModel>
        /// </summary>
        /// <typeparam name="TEntity">源对象</typeparam>
        /// <typeparam name="TModel">要映射的目标类型</typeparam>
        /// <param name="entities">源对象</param>
        /// <param name="models">要映射的目标类型</param>
        /// <returns>更新后的目标类型对象</returns>
        public static IEnumerable<TModel> ToModels<TEntity, TModel>(this IEnumerable<TEntity> entities, IEnumerable<TModel> models)
            where TEntity : IBaseEntity
            where TModel : IBaseDtoModel
        {
            if (entities == null)
                return default(IEnumerable<TModel>);
            if (models == null)
                return default(IEnumerable<TModel>);

            return entities.MapTo(models);
        }
        #endregion

        #endregion

        #region ToTEntity

        #region View
        /// <summary>
        /// 将ViewModel实体转换 BaseEntity
        /// </summary>
        /// <typeparam name="TEntity">要映射的目标类型</typeparam>
        /// <param name="baseNopEntity">源对象</param>
        /// <returns>目标类型的对象</returns>
        public static TEntity ToEntity<TEntity>(this IBaseDtoModel baseNopEntity) where TEntity : IBaseEntity
        {
            if (baseNopEntity == null)
                return default(TEntity);
            return baseNopEntity.Map<TEntity>();
        }

        /// <summary>
        ///  将ViewModel实体转换 BaseEntity
        /// </summary>
        /// <typeparam name="TModel">源对象</typeparam>
        /// <typeparam name="TEntity">要映射的目标类型</typeparam>
        /// <param name="model">源对象</param>
        /// <param name="baseNopEntity">要映射的目标类型</param>
        /// <returns></returns>
        public static TEntity ToEntity<TModel, TEntity>(this TModel model, TEntity baseNopEntity)
            where TModel : IBaseDtoModel
            where TEntity : IBaseEntity
        {
            if (model == null)
                return default(TEntity);
            if (baseNopEntity == null)
                return default(TEntity);
            return model.MapTo(baseNopEntity);
        }
        #endregion

        #region IEnumerable

        /// <summary>
        /// 将ViewModel实体转换 BaseEntity
        /// </summary>
        /// <typeparam name="TEntity">要映射的目标类型</typeparam>
        /// <param name="baseNopEntity">源对象</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> ToEntitys<TEntity>(this IEnumerable<IBaseDtoModel> baseNopEntity)
            where TEntity : IBaseEntity
        {
            if (baseNopEntity == null)
                return default(IEnumerable<TEntity>);
            return baseNopEntity.Map<IEnumerable<TEntity>>();

        }

        /// <summary>
        /// 将ViewModel实体转换 BaseEntity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> ToEntitys<TEntity, TModel>(this IEnumerable<TModel> model, IEnumerable<TEntity> entitys)
            where TModel : IBaseDtoModel
            where TEntity : IBaseEntity
        {
            if (model == null)
                return default(IEnumerable<TEntity>);
            if (entitys == null)
                return default(IEnumerable<TEntity>);

            return model.MapTo(entitys);
        }


        #endregion
        #endregion

    }
}
