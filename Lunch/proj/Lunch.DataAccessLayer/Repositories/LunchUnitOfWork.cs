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
        private bool disposed = false;


        private DishCategoryRepository _dishCategoryRepository;
        private DishRepository _dishRepository;
        private MenuRepository _menuRepository;
        private UserMenuRepository _userMenuRepository;
        #endregion

        #region Repositoies        
        public DishCategoryRepository DishCategoryRepository
        {
            get
            {
                if (this._dishCategoryRepository == null)
                {
                    this._dishCategoryRepository = new DishCategoryRepository(_dbContext);
                }
                return _dishCategoryRepository;
            }
        }

        public DishRepository DishRepository
        {
            get
            {
                if (this._dishRepository == null)
                {
                    this._dishRepository = new DishRepository(_dbContext);
                }
                return _dishRepository;
            }
        }

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

        public UserMenuRepository UserMenuRepository
        {
            get
            {
                if (this._userMenuRepository == null)
                {
                    this._userMenuRepository = new UserMenuRepository(_dbContext);
                }
                return _userMenuRepository;
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
    }
}
