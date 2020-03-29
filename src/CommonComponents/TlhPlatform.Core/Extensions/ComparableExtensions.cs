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
    /// �û��Ự
    /// </summary>
    public interface ISession : ISingletonDependency
    {
        /// <summary>
        /// �Ƿ���֤
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// �û���ʶ
        /// </summary>
        string UserId { get; }
    }
    /// <summary>
    /// �û��Ự
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// ���û��Ự
        /// </summary>
        public static readonly ISession Null = NullSession.Instance;

        /// <summary>
        /// �û��Ự
        /// </summary>
        public static readonly ISession Instance = new Session();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string UserId => throw new NotImplementedException();
    }

    /// <summary>
    /// ���û��Ự
    /// </summary>
    public class NullSession : ISession
    {
        /// <summary>
        /// �Ƿ���֤
        /// </summary>
        public bool IsAuthenticated => false;

        /// <summary>
        /// �û����
        /// </summary>
        public string UserId => string.Empty;

        /// <summary>
        /// ���û��Ựʵ��
        /// </summary>
        public static readonly ISession Instance = new NullSession();
    }

    /// <summary>
    /// ���ù�������
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="type">����</param>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// ���з�
        /// </summary>
        public static string Line => Environment.NewLine;

         
    }

    /// <summary>
    /// ö�ٲ���
    /// </summary>
    public static partial class EnumExtes
    {
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <typeparam name="TEnum">ö������</typeparam>
        /// <param name="member">��Ա����ֵ,����:Enum1ö���г�ԱA=0,����"A"��"0"��ȡ Enum1.A</param>
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
        /// ��ȡ��Ա��
        /// </summary>
        /// <typeparam name="TEnum">ö������</typeparam>
        /// <param name="member">��Ա����ֵ��ʵ������,����:Enum1ö���г�ԱA=0,����Enum1.A��0,��ȡ��Ա��"A"</param>
        public static string GetName<TEnum>(object member)
        {
            return GetName(Common.GetType<TEnum>(), member);
        }

        /// <summary>
        /// ��ȡ��Ա��
        /// </summary>
        /// <param name="type">ö������</param>
        /// <param name="member">��Ա����ֵ��ʵ������</param>
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
        /// ��ȡ��Աֵ
        /// </summary>
        /// <typeparam name="TEnum">ö������</typeparam>
        /// <param name="member">��Ա����ֵ��ʵ�����ɣ�����:Enum1ö���г�ԱA=0,�ɴ���"A"��0��Enum1.A����ȡֵ0</param>
        public static int GetValue<TEnum>(object member)
        {
            return GetValue(Common.GetType<TEnum>(), member);
        }

        /// <summary>
        /// ��ȡ��Աֵ
        /// </summary>
        /// <param name="type">ö������</param>
        /// <param name="member">��Ա����ֵ��ʵ������</param>
        public static int GetValue(Type type, object member)
        {
            string value = member.SafeString();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(member));
            return (int)System.Enum.Parse(type, member.ToString(), true);
        }

        
    }


    /// <summary>
    /// ����
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// Ĭ������
        /// </summary>
        internal static readonly Container DefaultContainer = new Container();
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="name">��������</param>
        public static T Create<T>(string name = null)
        {
            return DefaultContainer.Create<T>(name);
        }
    }
    /// <summary>
    /// Autofac��������
    /// </summary>
    internal class Container : IContainer
    {
        /// <summary>
        /// ����
        /// </summary>
        private Autofac.IContainer _container;
        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="name">��������</param>
        public T Create<T>(string name = null)
        {
            return (T)Create(typeof(T), name);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="name">��������</param>
        public object Create(Type type, string name = null)
        {
            return  GetService(type, name);
        }
        /// <summary>
        /// ��ȡ����
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
    /// ����
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="name">��������</param>
        T Create<T>(string name = null);
    }

}