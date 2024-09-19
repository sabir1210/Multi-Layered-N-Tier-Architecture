using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NTier.DataAccess.Data;
using NTier.DataAccess.Repositories;

namespace NTier.DataAccess.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction _transaction;
        private readonly ILogger<UnitOfWork> _logger;
        public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }
            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                _logger.LogInformation("Transaction started");
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
                _logger.LogInformation("Transaction committed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error committing transaction");
                await RollbackAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    _logger.LogInformation("Transaction rolled back");
                    await DisposeTransactionAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back transaction");
                throw;
            }
        }

        public async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
