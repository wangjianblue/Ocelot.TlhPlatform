using System.Threading.Tasks;

namespace TlhPlatform.Core.Domain
{
    public interface IUnitOfWork
	{
		int Commit();
        Task<int> CommitAsync();
	}
}
