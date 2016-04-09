using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunch.DataAccess;
using Lunch.Model;

namespace Lunch.DataAccessLayer.Repositories
{
    public class DishPictureRepository:RepositoryBase<DishPicture>
    {
         #region Constructor
        internal DishPictureRepository(Entities context)
            : base(context)
        {
        }
        #endregion


        #region Public methods
        public void Upsert(DishPicture entity)
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

        public DishPicture GetDishPictureById(int id)
        {
            return this.DbContext.Set<DishPicture>().FirstOrDefault(d => d.Id == id);
        }
        #endregion
    }
}
