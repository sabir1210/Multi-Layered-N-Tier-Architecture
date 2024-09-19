using NTier.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
