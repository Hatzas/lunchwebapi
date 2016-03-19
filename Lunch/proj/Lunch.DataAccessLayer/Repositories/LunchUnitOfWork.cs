using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunch.DataAccess;

namespace Lunch.DataAccessLayer.Repositories
{
    public class LunchUnitOfWork : IDisposable
    {
        #region Private
        private readonly Entities _dbContext;
        private MenuRepository _menuRepository;
        private bool disposed = false;       
        #endregion

        #region Repositoies
        public MenuRepository MenuRepository
        {
            get
            {
                if (this._menuRepository == null)
                {
                    this._menuRepository = new MenuRepository(_dbContext);
                }
                return _menuRepository;
            }
        }

        #endregion

        public bool AutoDetectChangesEnabled
        {
            get
            {
                return this._dbContext.Configuration.AutoDetectChangesEnabled;
            }

            set
            {
                this._dbContext.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        public bool LazyLoadingEnabled
        {
            get
            {
                return this._dbContext.Configuration.LazyLoadingEnabled;
            }

            set
            {
                this._dbContext.Configuration.LazyLoadingEnabled = value;
            }
        }

        public LunchUnitOfWork()
        {
            _dbContext = new Entities();
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        internal Entities DbContext
        {
            get { return _dbContext; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
