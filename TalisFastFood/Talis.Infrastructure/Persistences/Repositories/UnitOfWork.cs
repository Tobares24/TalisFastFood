using System.Data;
using System.Data.SqlClient;
using Talis.Infrastructure.Persistences.Data;
using Talis.Infrastructure.Persistences.Interfaces;

namespace Talis.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITalisContext _context;
        private readonly IDbTransaction _transaction;
        private readonly SqlConnection _connection;
        private bool _disposed;
        public IProductRepository Product { get; private set; }

        public UnitOfWork(ITalisContext context)
        {
            this._context = context;
            this._connection = this._context.GetSqlConnection();
            this._connection.Open();
            this._transaction = this._connection.BeginTransaction();
            this._disposed = false;
            this.Product = new ProductRepository(context);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                    _context.GetSqlConnection().Dispose();
                }
                _disposed = true;
            }
        }
    }
}
