using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TlhPlatform.Infrastructure.AutoMapper
{
    public static class ConfigureException
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperException(this IServiceCollection services)
        {
            var config = new MapperConfiguration(configuration =>
            { 
                #region 采用反射映射实体类， 必须继承BaseEntity 和Base.EntityModel 抽象类 
                new AutoMapperTypeFinder().FindAutoMapTypes(configuration); 
             
                #endregion
            });
            AutoMapperConfiguration.Init(config);
        } 
    }
}
