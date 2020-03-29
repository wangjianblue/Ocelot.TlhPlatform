using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TlhPlatform.Core.Reflection;
using TlhPlatform.Core.Reflection.Dependency;

namespace TlhPlatform.Core.Domain.Uow
{
    /// <summary>
    /// Unit of work manager.
    /// </summary>
    public class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
    {
        public IActiveUnitOfWork Current => throw new NotImplementedException();

        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            return null;
        }
    }
}
