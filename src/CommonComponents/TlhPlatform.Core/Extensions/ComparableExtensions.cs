using System;
using TlhPlatform.Core.Reflection;
using System.Reflection;
using Autofac;
using TlhPlatform.Core.Reflection.Dependency;

namespace TlhPlatform.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IComparable{T}"/>.
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Checks a value is between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to be checked</param>
        /// <param name="minInclusiveValue">Minimum (inclusive) value</param>
        /// <param name="maxInclusiveValue">Maximum (inclusive) value</param>
        public static bool IsBetween<T>(this T value, T minInclusiveValue, T maxInclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minInclusiveValue) >= 0 && value.CompareTo(maxInclusiveValue) <= 0;
        }
    }
 

    /// <summary>
    /// 用户会话
    /// </summary>
    public interface ISession : ISingletonDependency
    {
        /// <summary>
        /// 是否认证
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// 用户标识
        /// </summary>
        string UserId { get; }
    }
    /// <summary>
    /// 用户会话
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// 空用户会话
        /// </summary>
        public static readonly ISession Null = NullSession.Instance;

        /// <summary>
        /// 用户会话
        /// </summary>
        public static readonly ISession Instance = new Session();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string UserId => throw new NotImplementedException();
    }

    /// <summary>
    /// 空用户会话
    /// </summary>
    public class NullSession : ISession
    {
        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthenticated => false;

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId => string.Empty;

        /// <summary>
        /// 空用户会话实例
        /// </summary>
        public static readonly ISession Instance = new NullSession();
    }

    /// <summary>
    /// 常用公共操作
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 换行符
        /// </summary>
        public static string Line => Environment.NewLine;

         
    }

    /// <summary>
    /// 枚举操作
    /// </summary>
    public static partial class EnumExtes
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名或值,范例:Enum1枚举有成员A=0,则传入"A"或"0"获取 Enum1.A</param>
        public static TEnum Parse<TEnum>(object member)
        {
            string value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (typeof(TEnum).IsGenericType)
                    return default(TEnum);
                throw new ArgumentNullException(nameof(member));
            }
            return (TEnum)System.Enum.Parse(Common.GetType<TEnum>(), value, true);
        }

        /// <summary>
        /// 获取成员名
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名、值、实例均可,范例:Enum1枚举有成员A=0,则传入Enum1.A或0,获取成员名"A"</param>
        public static string GetName<TEnum>(object member)
        {
            return GetName(Common.GetType<TEnum>(), member);
        }

        /// <summary>
        /// 获取成员名
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        public static string GetName(Type type, object member)
        {
            if (type == null)
                return string.Empty;
            if (member == null)
                return string.Empty;
            if (member is string)
                return member.ToString();
            if (type.GetTypeInfo().IsEnum == false)
                return string.Empty;
            return System.Enum.GetName(type, member);
        }

        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名、值、实例均可，范例:Enum1枚举有成员A=0,可传入"A"、0、Enum1.A，获取值0</param>
        public static int GetValue<TEnum>(object member)
        {
            return GetValue(Common.GetType<TEnum>(), member);
        }

        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        public static int GetValue(Type type, object member)
        {
            string value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(member));
            return (int)System.Enum.Parse(type, member.ToString(), true);
        }

        
    }


    /// <summary>
    /// 容器
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// 默认容器
        /// </summary>
        internal static readonly Container DefaultContainer = new Container();
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">服务名称</param>
        public static T Create<T>(string name = null)
        {
            return DefaultContainer.Create<T>(name);
        }
    }
    /// <summary>
    /// Autofac对象容器
    /// </summary>
    internal class Container : IContainer
    {
        /// <summary>
        /// 容器
        /// </summary>
        private Autofac.IContainer _container;
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">服务名称</param>
        public T Create<T>(string name = null)
        {
            return (T)Create(typeof(T), name);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="name">服务名称</param>
        public object Create(Type type, string name = null)
        {
            return  GetService(type, name);
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        private object GetService(Type type, string name)
        {
            if (name == null)
                return _container.Resolve(type);
            return _container.ResolveNamed(name, type);
        }
        public void Dispose()
        {
            _container.Dispose();
        }
    }
    /// <summary>
    /// 容器
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="name">服务名称</param>
        T Create<T>(string name = null);
    }

}