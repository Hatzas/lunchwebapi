using Lunch.DataAccess;
using Lunch.Model;
using Lunch.Model.Extended;
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

        public List<MenuDetails> GetUserMenusDetailsByInterval(DateTime startDate, DateTime endDate)
        {
            var query = (from menu in this.DbContext.Menus
                         where menu.Date >= startDate && menu.Date <= endDate
                         select new MenuDetails
                         {
                             Id = menu.Id,
                             Date = menu.Date,
                             Serial = menu.Serial,
                             Dish = new DishDetails()
                             {
                                 Id = menu.Dish.Id,
                                 Name = menu.Dish.Name,
                                 Description = menu.Dish.Description,
                                 Type = menu.Dish.Type,
                                 DishPicture = new DishPictureDetails()
                                 {
                                     Id = menu.Dish.DishPicture.Id,
                                     Thumbnail = menu.Dish.DishPicture.Thumbnail,
                                 }
                             },
                             DishCategory = new DishCategoryDetails()
                             {
                                 Id = menu.DishCategory.Id,
                                 Name = menu.DishCategory.Name,
                             },
                             DishStatistics = menu.Dish.DishStatistics.Where(s => s.Rating != null).GroupBy(s => s.Rating)
                                                                      .Select(g => new DishStatsDetails { Rating = g.Key, RatingCount = g.Count() })
                         });

            return query.ToList();
        }
        #endregion
    }
}
