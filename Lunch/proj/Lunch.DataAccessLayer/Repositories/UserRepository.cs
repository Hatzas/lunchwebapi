using Lunch.DataAccess;
using Lunch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunch.DataAccessLayer.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        #region Constructor
        internal UserRepository(Entities context)
            : base(context)
        {
        }
        #endregion


        #region Public methods
        public void Upsert(User entity)
        {
            if (entity.Id == default(int))
            {
                this.InsertEntity(entity);
            }
            else
            {
                this.UpdateEntity(entity);
            }
        }
        #endregion
    }
}
