using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using JetBrains.Annotations;

namespace TlhPlatform.Core.Aspects
{
    /// <summary>
    /// 此类可用于在以下情况下提供操作
    /// 调用Dispose方法。
    /// </summary>
    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;

        /// <summary>
        /// Creates a new <see cref="DisposeAction"/> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction([CanBeNull] Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            // Interlocked prevents multiple execution of the _action.
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
