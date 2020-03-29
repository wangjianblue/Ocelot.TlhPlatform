using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TlhPlatform.Core.Domain.Uow
{
    /// <summary>
    /// 工作单元选项。
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 范围选项。
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// 这是UOW交易吗？
        /// 如果未提供，则使用默认值。
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// UOW超时以毫秒为单位。
        ///如果未提供，则使用默认值。
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 如果此UOW是事务性的，则此选项指示事务的隔离级别。
        ///如果未提供，则使用默认值
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 此选项应设置为<see cref =“TransactionScopeAsyncFlowOption.Enabled”/>
        ///如果工作单元用于异步范围。
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        /// <summary>
        /// 可用于启用/禁用某些过滤器。
        /// </summary>
        public List<DataFilterConfiguration> FilterOverrides { get; }

        /// <summary>
        /// 创建一个新的<see cref =“UnitOfWorkOptions”/>对象。
        /// </summary>
        public UnitOfWorkOptions()
        {
            FilterOverrides = new List<DataFilterConfiguration>();
        }

        internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
        {
            //TODO: Do not change options object..?

            if (!IsTransactional.HasValue)
            {
                IsTransactional = defaultOptions.IsTransactional;
            }

            if (!Scope.HasValue)
            {
                Scope = defaultOptions.Scope;
            }

            if (!Timeout.HasValue && defaultOptions.Timeout.HasValue)
            {
                Timeout = defaultOptions.Timeout.Value;
            }

            if (!IsolationLevel.HasValue && defaultOptions.IsolationLevel.HasValue)
            {
                IsolationLevel = defaultOptions.IsolationLevel.Value;
            }
        }

        internal void FillOuterUowFiltersForNonProvidedOptions(List<DataFilterConfiguration> filterOverrides)
        {
            foreach (var filterOverride in filterOverrides)
            {
                if (FilterOverrides.Any(fo => fo.FilterName == filterOverride.FilterName))
                {
                    continue;
                }

                FilterOverrides.Add(filterOverride);
            }
        }
    }
}
