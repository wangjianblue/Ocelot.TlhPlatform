﻿

namespace TlhPlatform.Core.Extensions
{
    /// <summary>
    /// 系统扩展 - 公共
    /// </summary>
    public static partial class Extensions {
        /// <summary>
        /// 安全获取值，当值为null时，不会抛出异常
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>( this T? value ) where T : struct {
            return value ?? default( T );
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static int Value( this System.Enum instance ) {
            return EnumExtensions.GetValue( instance.GetType(), instance );
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="instance">枚举实例</param>
        public static TResult Value<TResult>( this System.Enum instance ) {
            return ConvertHelper.To<TResult>( Value( instance ) );
        }

        /// <summary>
        /// 获取枚举描述,使用System.ComponentModel.Description特性设置描述
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static string Description( this System.Enum instance ) {
            return EnumExtensions.GetDescription( instance.GetType(), instance );
        } 
    }


}
