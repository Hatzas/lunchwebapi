using Lunch.DataAccess;
using Lunch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunch.DataAccessLayer.Repositories
{
    public class DishStatisticsRepository : RepositoryBase<DishStats>
    {
        #region Constructor
        internal DishStatisticsRepository(Entities context)
            : base(context)
        {
        }
        #endregion


        #region Public methods
        public void Upsert(DishStats entity)
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

        public DishStats GetDishStatistic(int dishId, int userId)
        {
            return DbContext.Set<DishStats>().FirstOrDefault(s => s.DishId == dishId && s.UserId == userId);
        }
        #endregion
    }
}
