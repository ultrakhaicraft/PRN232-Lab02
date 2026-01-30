using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Repo.Repository;

namespace PRN232.FUNewsManagement.Repo
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DbContext _context;
		private readonly Dictionary<Type, object> _repositories;
		private IDbContextTransaction? _transaction;
		private bool _disposed = false;

		public UnitOfWork(DbContext context)
		{
			_context = context;
			_repositories = new Dictionary<Type, object>();
		}

		public IGenericRepository<T> Repository<T>() where T : class
		{
			var type = typeof(T);

			if (!_repositories.ContainsKey(type))
			{
				var repositoryInstance = new GenericRepository<T>(_context);
				_repositories.Add(type, repositoryInstance);
			}

			return (IGenericRepository<T>)_repositories[type];
		}

		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			_transaction = await _context.Database.BeginTransactionAsync();
			return _transaction;
		}

		public async Task CommitAsync()
		{
			try
			{
				await SaveChangesAsync();

				if (_transaction != null)
				{
					await _transaction.CommitAsync();
				}
			}
			catch
			{
				await RollbackAsync();
				throw;
			}
			finally
			{
				if (_transaction != null)
				{
					await _transaction.DisposeAsync();
					_transaction = null;
				}
			}
		}

		public async Task RollbackAsync()
		{
			try
			{
				if (_transaction != null)
				{
					await _transaction.RollbackAsync();
				}
			}
			finally
			{
				if (_transaction != null)
				{
					await _transaction.DisposeAsync();
					_transaction = null;
				}

				DetachAllEntries();
			}
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public int SaveChanges()
		{
			return _context.SaveChanges();
		}

		public void DetachAllEntries()
		{
			var entries = _context.ChangeTracker.Entries()
				.Where(e => e.State != EntityState.Detached)
				.ToList();

			foreach (var entry in entries)
			{
				entry.State = EntityState.Detached;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_transaction?.Dispose();
					_context.Dispose();
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
