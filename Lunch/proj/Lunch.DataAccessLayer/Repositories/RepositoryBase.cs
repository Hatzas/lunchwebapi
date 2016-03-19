using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunch.DataAccess;

namespace Lunch.DataAccessLayer.Repositories
{
    public class RepositoryBase<T> where T : class
    {
        #region Protected Properties
        internal Entities DbContext;
        #endregion

        #region Constructor
        internal RepositoryBase(Entities context)
        {
            this.DbContext = context;
        }
        #endregion

        #region Public methods
        public T Find(int id)
        {
            return this.DbContext.Set<T>().Find(id);
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.Set<T>().Find(id);
            this.DbContext.Set<T>().Remove(entity);
        }

        public void DeleteEntity(T entity)
        {
            this.DbContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        #endregion

        #region Protected methods
        protected void InsertEntity(T entity)
        {
            this.DbContext.Set<T>().Add(entity);
        }

        protected void UpdateEntity(T entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Modified;
        }
        #endregion
    }
}
