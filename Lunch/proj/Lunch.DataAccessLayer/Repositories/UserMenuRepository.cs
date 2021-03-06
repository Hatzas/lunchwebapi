﻿using Lunch.DataAccess;
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

        public List<MenuDetails> GetUserMenusDetailsByInterval(DateTime startDate, DateTime endDate, string userName)
        {
            var query = (from menu in this.DbContext.Menus
                         where menu.Date >= startDate && menu.Date <= endDate
                         let user = this.DbContext.Users.FirstOrDefault(u => u.Name == userName)
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
                                                                      .Select(g => new DishStatsDetails { Rating = g.Key, RatingCount = g.Count() }),
                             SelectionCount = menu.Dish.DishStatistics.FirstOrDefault(s => s.UserId == user.Id).SelectionCount,
                             Selected = menu.UserMenus.Any(m => m.User.Name == userName)
                         });


            return query.ToList();
        }


        public List<MenuDetails> GetUserMenusDetailsByInterval2(DateTime startDate, DateTime endDate)
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
                                                                      .Select(g => new DishStatsDetails { Rating = g.Key, RatingCount = g.Count() }),
                             SelectionCount = 0,
                         });

            return query.ToList();
        }


        public List<UserMenu> GetUserMenuListByDates(List<DateTime> dateList)
        {
            var query = DbContext.Set<UserMenu>().Where(m => dateList.Contains(m.Date.Value));


            return query.ToList();
        }
        #endregion

        #region Private
        public List<UserMenu> GetUserMenusByInterval(DateTime startDateTime, DateTime endDateTime, int userId)
        {
            var query = DbContext.Set<UserMenu>().Where(m => m.Date.Value >= startDateTime && m.Date.Value <= endDateTime && m.UserId == userId);

            return query.ToList();
        }
        #endregion
    }
}
