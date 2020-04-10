using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using TlhPlatform.Core.Reflection;
using TlhPlatform.Core.Reflection.Dependency;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Infrastructure.AutoMapper
{
    public class AutoMapperTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 获取或设置，全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder allAssemblyFinder { get; set; }

        /// <summary>
        /// 获取或设置 全部程序集查找器
        /// </summary>
        public AutoMapperTypeFinder()
        {
            allAssemblyFinder = new AppDomainAllAssemblyFinder();
        }
        /// <summary>
        /// 查询所有依赖
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(ITransientDependency);
            Type[] types = allAssemblyFinder.FindAll(formCache: true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.HasAttribute<IgnoreDependencyAttribute>() && !type.IsAbstract && !type.IsInterface)
                .ToArray();
            return types;

        }

        /// <summary>
        /// 反射实体之间相互转化，命名规范数据库实体为 表名称  View实体为 表名+ViewModel或者表名+Model
        /// </summary>
        /// <param name="configuration"></param>
        public void FindAutoMapTypes(IMapperConfigurationExpression configuration)
        {
            var assembly = allAssemblyFinder.FindAll(formCache: true).SelectMany(p => p.GetTypes());
            var baseTypes = assembly.Where(p => typeof(IBaseEntity).IsAssignableFrom(p) && p.IsClass);
            foreach (var item in baseTypes)
            {
                var model = assembly.FirstOrDefault(p => typeof(IBaseDtoModel).IsAssignableFrom(p) && p.Name == ($"{item.Name}"));
                if (model != null)
                {
                    configuration.CreateMap(item, model);//从实体到Model
                    configuration.CreateMap(model, item);//从Model到实体
                }
                var viewmodel = assembly.FirstOrDefault(p => typeof(IBaseDtoModel).IsAssignableFrom(p) && p.Name == ($"{item.Name}{"Dto"}"));
                if (viewmodel != null)
                {
                    configuration.CreateMap(item, viewmodel);//从实体到Model
                    configuration.CreateMap(viewmodel, item);//从Model到实体

                }
            }
        }
    }
}
