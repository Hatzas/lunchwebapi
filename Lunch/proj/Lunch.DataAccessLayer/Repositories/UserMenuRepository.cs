using Lunch.DataAccess;
using Lunch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunch.DataAccessLayer.Repositories
{
    public class UserMenuRepository : RepositoryBase<UserMenu>
    {
        #region Constructor
        internal UserMenuRepository(Entities context)
            : base(context)
        {
        }
        #endregion


        #region Public methods
        public void Upsert(UserMenu entity)
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
