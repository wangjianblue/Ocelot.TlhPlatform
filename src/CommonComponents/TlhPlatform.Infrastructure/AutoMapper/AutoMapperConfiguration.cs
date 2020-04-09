using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace TlhPlatform.Infrastructure.AutoMapper
{
    /// <summary>
    /// 映射器
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// 映射器
        /// </summary>
        public static IMapper Mapper { get; set; }
        /// <summary>
        /// 映射器配置
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mapperConfiguration"></param>
        public static void Init(MapperConfiguration mapperConfiguration)
        {
            MapperConfiguration = mapperConfiguration;
            Mapper = MapperConfiguration.CreateMapper();
        }
    }
}
